using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using SkiaSharp;

// IconForge - paints a spell icon for every spell that does not have one.
//
// This calls Google's Gemini image models, not Claude: the Anthropic API has no image generation
// at all, so painted art has to come from somewhere else. An earlier version had Claude write SVG
// and rasterised that here, which produced clean vector tiles but never looked painted - hand
// written vector paths have a hard ceiling against real brushwork.
//
// Images come back at 512px and are downscaled to 44px here. That downscale is the whole design
// constraint: a busy 512px painting becomes mush at 44px, so the prompt asks for a bold simple
// subject rendered in a painterly style, rather than for detail that cannot survive the shrink.
//
// Resumable by design: it skips any spell that already has a PNG, so an interrupted run continues
// where it stopped, a single spell can be redrawn by deleting its file, and hand-drawn art is
// never overwritten.

const string Endpoint = "https://generativelanguage.googleapis.com/v1beta/interactions";
const string Model = "gemini-3.1-flash-image";

// 512 is deliberate. The icon is displayed at 44px, so anything larger is detail thrown away by
// the downscale - it costs more, takes longer, and does not improve the result.
const string ImageSize = "0.5K";

// The house style lives in the Style class at the foot of this file - a top-level const is not
// visible to the Spell record, and C# requires type declarations to follow the entry point.

if (args.Contains("--selftest"))
{
    return SelfTest();
}

var spellData = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Data", "DnDSpells.xml");
var iconDirectory = @"C:\ClassicUO\src\ClassicUO.Client\Data\SpellIcons";

// Both paths are overridable, because the client lives in a sibling repository whose location is
// a local choice rather than something this tool can know.
//
// Parsed in one pass rather than by filtering out anything starting with "--", because a flag's
// VALUE does not start with "--" either: `--limit 6` left a bare "6" in the positional list, which
// was then read as the spell-data path, and the tool went looking for spells in a file called "6".
int limit = 0;
var positional = new List<string>();

for (int i = 0; i < args.Length; ++i)
{
    switch (args[i])
    {
        case "--selftest":
            break;

        case "--limit":
            if (i + 1 >= args.Length || !int.TryParse(args[i + 1], out limit) || limit <= 0)
            {
                Console.Error.WriteLine("--limit needs a positive number, e.g. --limit 6");
                return 1;
            }

            ++i; // the value belongs to this flag, not to the positional list
            break;

        default:
            if (args[i].StartsWith("--"))
            {
                Console.Error.WriteLine($"Unknown option {args[i]}");
                return 1;
            }

            positional.Add(args[i]);
            break;
    }
}

if (positional.Count > 0) spellData = positional[0];
if (positional.Count > 1) iconDirectory = positional[1];

spellData = Path.GetFullPath(spellData);

if (!File.Exists(spellData))
{
    Console.Error.WriteLine($"No spell data at {spellData}");
    return 1;
}

string? apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")
              ?? Environment.GetEnvironmentVariable("GOOGLE_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("Set GEMINI_API_KEY (get one free at https://aistudio.google.com/apikey).");
    Console.Error.WriteLine("On Windows, `setx` only reaches NEW processes - open a fresh terminal after setting it.");
    return 1;
}

// A warning rather than a refusal: the prefix is a long-standing Google convention, not a promise,
// and refusing a key that turns out to be valid would be worse than a line of noise. But an OAuth
// token pasted in place of an API key fails with a bare 403, and guessing why costs a round trip.
if (!apiKey.StartsWith("AIza"))
{
    Console.Error.WriteLine(
        $"Warning: GEMINI_API_KEY starts '{apiKey[..Math.Min(4, apiKey.Length)]}...' - AI Studio keys "
        + "normally start 'AIza'. If authentication fails, check you took the key from "
        + "https://aistudio.google.com/apikey rather than an OAuth token from elsewhere.");
}

Directory.CreateDirectory(iconDirectory);

var document = new XmlDocument();
document.Load(spellData);

var pending = new List<Spell>();
int total = 0;

foreach (XmlElement element in document.SelectNodes("//spell")!)
{
    string name = element.GetAttribute("name");

    if (string.IsNullOrEmpty(name))
    {
        continue;
    }

    ++total;

    // Blindness/Deafness and Enlarge/Reduce have a slash in their name and no file can. The client
    // applies the same substitution when it looks art up, so the two agree.
    string fileName = Regex.Replace(name, @"[\\/:*?""<>|]", "-");
    string path = Path.Combine(iconDirectory, fileName + ".png");

    if (File.Exists(path))
    {
        continue;
    }

    pending.Add(new Spell(
        name,
        path,
        element.GetAttribute("school"),
        element.GetAttribute("kind"),
        element.GetAttribute("description")));
}

if (limit > 0 && pending.Count > limit)
{
    pending = pending.Take(limit).ToList();
    Console.WriteLine($"{pending.Count} icon(s) this run (--limit), out of {total} spell(s).");
}
else
{
    Console.WriteLine($"{pending.Count} of {total} spell(s) need art.");
}

if (pending.Count == 0)
{
    return 0;
}

using var http = new HttpClient { Timeout = TimeSpan.FromMinutes(3) };
http.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);

// Three at a time. Image generation is slower and more rate-limited than text, and the free tier
// is tighter still - raise it once you know your quota holds.
using var throttle = new SemaphoreSlim(3);

int drawn = 0, failed = 0;
object consoleLock = new();

var work = pending.Select(async spell =>
{
    await throttle.WaitAsync();

    try
    {
        byte[]? image = await Paint(http, spell);

        if (image == null)
        {
            Interlocked.Increment(ref failed);
            return;
        }

        if (!Downscale(image, spell.Path, explain: true))
        {
            lock (consoleLock)
            {
                Console.Error.WriteLine($"  {spell.Name}: the image did not convert");
            }

            Interlocked.Increment(ref failed);
            return;
        }

        int done = Interlocked.Increment(ref drawn);

        lock (consoleLock)
        {
            Console.WriteLine($"[{done}/{pending.Count}] {spell.Name}");
        }
    }
    finally
    {
        throttle.Release();
    }
});

await Task.WhenAll(work);

Console.WriteLine($"\nPainted {drawn} icon(s), {failed} failed.");
Console.WriteLine(failed > 0 ? "Re-run to retry the failures - finished icons are skipped." : "Done.");

return failed > 0 ? 1 : 0;

/// <summary>Asks Gemini for one icon. Returns the raw image bytes, or null if it could not.</summary>
static async Task<byte[]?> Paint(HttpClient http, Spell spell)
{
    var request = new
    {
        model = Model,
        input = new object[]
        {
            new { type = "text", text = spell.Describe() },
        },
        response_format = new
        {
            type = "image",
            mime_type = "image/jpeg",
            aspect_ratio = "1:1",
            image_size = ImageSize,
        },
    };

    string body = JsonSerializer.Serialize(request);

    for (int attempt = 1; attempt <= 3; ++attempt)
    {
        try
        {
            using var content = new StringContent(body, Encoding.UTF8, "application/json");
            using var response = await http.PostAsync(Endpoint, content);

            string text = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // A bad key fails identically on every spell, so retrying it 229 times is a wall
                // of the same error and no icons. Stop instead.
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Console.Error.WriteLine($"\nAuthentication failed: {Summarise(text)}");
                    Console.Error.WriteLine("Check GEMINI_API_KEY.");

                    Environment.Exit(1);
                }

                Console.Error.WriteLine(
                    $"  {spell.Name}: {(int)response.StatusCode} {Summarise(text)} (attempt {attempt})");

                if (attempt < 3)
                {
                    // Rate limits want a real pause, not an immediate retry.
                    await Task.Delay(TimeSpan.FromSeconds(5 * attempt));
                    continue;
                }

                return null;
            }

            byte[]? image = ExtractImage(text);

            if (image != null)
            {
                return image;
            }

            Console.Error.WriteLine($"  {spell.Name}: no image in the reply (attempt {attempt})");
        }
        catch (Exception e) when (attempt < 3)
        {
            Console.Error.WriteLine($"  {spell.Name}: {e.Message} (attempt {attempt})");
            await Task.Delay(TimeSpan.FromSeconds(5 * attempt));
        }
    }

    return null;
}

/// <summary>
/// Digs the base64 image out of the response.
/// <para>
/// Written as a search for any sufficiently long base64 string rather than as a walk down a fixed
/// path, because this API's response shape has changed before and a tool that breaks on a renamed
/// wrapper field is a tool that breaks again. The size floor is what makes it safe: no id, token,
/// or short field reaches a few thousand characters.
/// </para>
/// </summary>
static byte[]? ExtractImage(string json)
{
    try
    {
        using var document = JsonDocument.Parse(json);

        string? best = null;

        Walk(document.RootElement, ref best);

        return best == null ? null : Convert.FromBase64String(best);
    }
    catch
    {
        return null;
    }

    static void Walk(JsonElement element, ref string? best)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    Walk(property.Value, ref best);
                }

                break;

            case JsonValueKind.Array:
                foreach (var item in element.EnumerateArray())
                {
                    Walk(item, ref best);
                }

                break;

            case JsonValueKind.String:
                string? value = element.GetString();

                // Longest wins: if a response ever carries a thumbnail alongside the real image,
                // the real one is the bigger string.
                if (value is { Length: > 2000 } && (best == null || value.Length > best.Length)
                    && IsBase64(value))
                {
                    best = value;
                }

                break;
        }
    }

    static bool IsBase64(string value)
    {
        // Cheap enough to run on every long string, and it rejects prose and URLs immediately.
        foreach (char c in value)
        {
            if (!char.IsAsciiLetterOrDigit(c) && c != '+' && c != '/' && c != '=' && c != '-'
                && c != '_' && !char.IsWhiteSpace(c))
            {
                return false;
            }
        }

        return true;
    }
}

/// <summary>
/// Converts a generated image to the 44x44 PNG the client's loader wants.
/// <para>
/// The resampler matters more than it looks. Going from 512 to 44 throws away 99% of the pixels,
/// and a naive nearest-neighbour pick produces aliased noise from a painting that looked fine at
/// full size. Mitchell cubic averages the neighbourhood, which is what keeps the downscale
/// readable.
/// </para>
/// </summary>
static bool Downscale(byte[] image, string path, bool explain = false)
{
    try
    {
        using var source = SKBitmap.Decode(image);

        if (source == null)
        {
            if (explain)
            {
                Console.Error.WriteLine("  the bytes did not decode as an image");
            }

            return false;
        }

        var info = new SKImageInfo(44, 44, SKColorType.Rgba8888, SKAlphaType.Premul);

        using var scaled = source.Resize(info, new SKSamplingOptions(SKCubicResampler.Mitchell));

        if (scaled == null)
        {
            if (explain)
            {
                Console.Error.WriteLine("  the resize failed");
            }

            return false;
        }

        using var encoded = SKImage.FromBitmap(scaled);
        using var data = encoded.Encode(SKEncodedImageFormat.Png, 100);
        using var file = File.Create(path);

        data.SaveTo(file);

        return true;
    }
    catch (Exception e)
    {
        if (explain)
        {
            Console.Error.WriteLine($"  {e.GetType().Name}: {e.Message}");
        }

        return false;
    }
}

/// <summary>
/// Exercises the half that needs no API key: image bytes in, 44x44 PNG out. Worth having
/// separately, because a broken converter and a broken prompt look identical from the outside -
/// both produce no icon - and only one of them costs money to diagnose.
/// </summary>
static int SelfTest()
{
    // A stand-in for a generated painting: a large image with a distinct centre and corner, so
    // the downscale has something whose survival can be checked.
    using var large = new SKBitmap(512, 512, SKColorType.Rgba8888, SKAlphaType.Premul);
    using (var canvas = new SKCanvas(large))
    {
        canvas.Clear(new SKColor(0x1A, 0x0C, 0x04));

        using var glow = new SKPaint { Color = new SKColor(0xFF, 0xC8, 0x40), IsAntialias = true };

        canvas.DrawCircle(256, 256, 150, glow);
    }

    using var sourceData = SKImage.FromBitmap(large).Encode(SKEncodedImageFormat.Jpeg, 92);

    string probe = Path.Combine(Path.GetTempPath(), "iconforge-selftest.png");

    if (!Downscale(sourceData.ToArray(), probe, explain: true))
    {
        Console.Error.WriteLine("FAIL: the converter produced nothing.");
        return 1;
    }

    using var check = SKBitmap.Decode(probe);

    if (check is null || check.Width != 44 || check.Height != 44)
    {
        Console.Error.WriteLine($"FAIL: expected a 44x44 image, got {check?.Width}x{check?.Height}.");
        return 1;
    }

    var corner = check.GetPixel(1, 1);
    var centre = check.GetPixel(22, 22);

    // Both opaque proves the tile fills the canvas; the colour gap proves the downscale carried
    // the content through rather than flooding one average colour over everything.
    bool opaque = corner.Alpha > 200 && centre.Alpha > 200;
    bool contentSurvived = centre.Red - corner.Red > 80;

    if (!opaque || !contentSurvived)
    {
        Console.Error.WriteLine(
            $"FAIL: corner alpha {corner.Alpha}, centre alpha {centre.Alpha} (want both >200); " +
            $"red delta {centre.Red - corner.Red} (want >80).");

        return 1;
    }

    Console.WriteLine($"Converter OK: 512 -> 44x44, content survived the downscale. {probe}");
    return 0;
}

/// <summary>Trims an error body to something readable on one line.</summary>
static string Summarise(string body)
{
    body = Regex.Replace(body, @"\s+", " ").Trim();

    return body.Length > 200 ? body[..200] + "..." : body;
}

/// <summary>One spell, and the art direction that can be derived from its own row.</summary>
record Spell(string Name, string Path, string School, string Kind, string Description)
{
    public string Describe()
    {
        var brief = new StringBuilder();

        brief.Append($"A spell icon for the Dungeons & Dragons spell \"{Name}\".");

        // The description is written for a player, which makes it better art direction than
        // anything derivable from the numbers.
        if (!string.IsNullOrEmpty(Description))
        {
            brief.Append($" The spell: {Description.TrimEnd('.')}.");
        }

        brief.Append($" Subject: {Subject()}");
        brief.Append($" The field and border are {Palette()}.");

        string? element = Element();

        if (element != null)
        {
            brief.Append($" The subject glows with {element}.");
        }

        brief.Append("\n\n").Append(Style.Guide);

        return brief.ToString();
    }

    /// <summary>
    /// What to actually draw. The school implies a family of imagery, which is what keeps a set
    /// of 229 looking related rather than like 229 unrelated commissions.
    /// </summary>
    private string Subject() => School switch
    {
        "Abjuration" => "a warding shield, sigil, or barrier of protective force.",
        "Conjuration" => "something being summoned into being through a portal or circle.",
        "Divination" => "an all-seeing eye, a scrying orb, or a revealed sigil.",
        "Enchantment" => "a symbol of influence over the mind - a charmed heart, a spiral, a crown.",
        "Evocation" => "raw elemental force erupting - a blast, a bolt, or a burst of energy.",
        "Illusion" => "something half-real and shifting - a mask, a mirrored figure, a fading form.",
        "Necromancy" => "a grim emblem of death - a skull, a skeletal hand, a guttering soul.",
        "Transmutation" => "matter caught mid-change - a form warping, flowing, or transmuting.",
        _ => "an arcane emblem of the spell's power.",
    };

    /// <summary>The school's colour, which sets the tile and its border.</summary>
    private string Palette() => School switch
    {
        "Abjuration" => "deep sapphire blue with a steel-blue metal border",
        "Conjuration" => "deep forest green with a brass border",
        "Divination" => "deep indigo violet with a silver border",
        "Enchantment" => "deep magenta rose with a rose-gold border",
        "Evocation" => "dark ember red-orange with a bronze border",
        "Illusion" => "dusky purple with a pewter border",
        "Necromancy" => "near-black lit with sickly green, with a verdigris border",
        "Transmutation" => "warm dark bronze-brown with a copper border",
        _ => "deep slate grey with an iron border",
    };

    /// <summary>
    /// The colour a spell reads as, taken from what it does rather than its school - a player
    /// recognises a fire spell by it being orange, not by it being Evocation.
    /// </summary>
    private string? Element()
    {
        string n = Name.ToLowerInvariant();

        if (Regex.IsMatch(n, "fire|flame|burn|scorch|meteor")) return "orange and gold flame";
        if (Regex.IsMatch(n, "frost|ice|cold|freez")) return "pale blue ice and frost";
        if (Regex.IsMatch(n, "lightning|shock|thunder|storm|electr")) return "white-blue lightning";
        if (Regex.IsMatch(n, "acid|poison|venom|blight|contagion")) return "sickly green corrosion";
        if (Regex.IsMatch(n, "necro|death|undead|wither|grave|bone")) return "cold green-black necrotic energy";
        if (Regex.IsMatch(n, "holy|divine|sacred|radiant|sun|daylight|bless")) return "warm golden radiance";
        if (Regex.IsMatch(n, "heal|cure|restor|revivify|resurrect|mend")) return "soft green-white healing light";
        if (Regex.IsMatch(n, "psychic|mind|charm|dominate|suggest|fear")) return "violet psychic energy";

        return Kind switch
        {
            "Healing" => "soft green-white healing light",
            "Resistance" or "ArmorClass" => "translucent blue protective light",
            "Revive" => "warm golden radiance",
            "Light" => "clear white light",
            _ => null,
        };
    }
}

/// <summary>
/// The one description of the house style, shared by every icon.
/// <para>
/// In its own type because the <see cref="Spell"/> record needs it and a top-level const is not
/// visible outside the entry point. Kept in a single place because consistency across the set
/// matters more than any individual icon: 229 icons that agree with each other read as a game,
/// and 229 that do not read as a clip-art folder.
/// </para>
/// </summary>
static class Style
{
    public const string Guide = """
        Painted fantasy RPG spell icon, in the style of Baldur's Gate, Pathfinder, and Diablo
        spellbook art. Digital oil painting with visible brushwork, rich impasto texture, and
        dramatic single-source lighting. Deep shadows, luminous highlights, weathered and physical.

        Composition: one bold central subject on a dark, richly coloured square field, framed by a
        narrow ornate metallic border like an enamelled medallion. The subject fills the middle of
        the frame and reads instantly as a silhouette.

        CRITICAL - this icon is displayed at 44x44 pixels. Keep the composition simple and the
        silhouette strong. Do not add small ornamental detail, fine filigree, background scenery,
        or multiple competing elements - they turn to noise at that size. Bold shapes, high
        contrast, few elements, painted richly.

        No text, no letters, no numbers, no watermark, no signature, no UI elements, no borders
        outside the medallion frame.
        """;
}
