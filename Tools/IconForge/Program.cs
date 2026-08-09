using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Anthropic;
using Anthropic.Models.Messages;
using SkiaSharp;
using Svg.Skia;

// IconForge - draws a spell icon for every spell that does not have one.
//
// Claude cannot produce a raster image; there is no API that returns a PNG. What it can do is
// write SVG, which is text, and which rasterises to a PNG cleanly at any size. So this asks for
// vector art and converts it here. The result is stylised rather than painted - closer to a crisp
// game icon than to an illustration - but it is real art rather than a coloured glyph, and it is
// consistent across all 229 spells because one prompt describes the whole set.
//
// Resumable by design: it skips any spell that already has a PNG, so an interrupted run continues
// where it stopped, a single spell can be redrawn by deleting its file, and hand-drawn art is
// never overwritten.

const string Model = "claude-opus-5";

// The one description of the house style, shared by every icon. Kept in a single place because
// consistency across the set matters more than any individual icon: 229 icons that agree with
// each other read as a game, and 229 that do not read as a clip-art folder.
const string StyleGuide = """
    You are drawing icons for a Dungeons & Dragons spellbook in a fantasy game client.

    Output a single SVG and nothing else. No markdown fence, no commentary, no explanation.

    Requirements:
    - Exactly `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 44 44" width="44" height="44">`
    - The icon is viewed at 44x44 pixels. Shapes must read at that size: bold silhouettes, few
      elements, strong contrast. Fine detail disappears and becomes mud.
    - Transparent background. Do not paint a background rectangle - the book page shows through.
    - A single centred subject that fills most of the frame, with a small margin.
    - Rich colour: use gradients (`<radialGradient>`, `<linearGradient>`) for depth and glow, and
      a brighter core against darker outer tones so the subject looks lit from within.
    - A dark outline or dark outer shadow so the icon separates from a light page.
    - No text, no letters, no numbers, no borders, no frames.
    - Use only plain SVG shapes, paths, and gradients. No filters, no external references,
      no embedded images, no scripts.
    """;

// --selftest exercises the half of this tool that needs no API key: SVG in, 44x44 PNG out. Worth
// having separately, because a broken rasteriser and a broken prompt look identical from the
// outside - both produce no icon - and only one of them costs money to diagnose.
if (args.Contains("--selftest"))
{
    const string sample = """
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 44 44" width="44" height="44">
          <defs><radialGradient id="g"><stop offset="0%" stop-color="#fff2b0"/>
          <stop offset="60%" stop-color="#ff9a1f"/><stop offset="100%" stop-color="#7a2a00"/>
          </radialGradient></defs>
          <circle cx="22" cy="22" r="16" fill="url(#g)" stroke="#2b0f00" stroke-width="2"/>
        </svg>
        """;

    string probe = Path.Combine(Path.GetTempPath(), "iconforge-selftest.png");

    if (!Rasterise(sample, probe, explain: true))
    {
        Console.Error.WriteLine("FAIL: the rasteriser produced nothing.");
        return 1;
    }

    using var check = SKBitmap.Decode(probe);

    if (check is null || check.Width != 44 || check.Height != 44)
    {
        Console.Error.WriteLine($"FAIL: expected a 44x44 image, got {check?.Width}x{check?.Height}.");
        return 1;
    }

    // A transparent corner and an opaque centre together prove the alpha channel survived and
    // something was actually drawn - either alone would pass on a blank or a solid image.
    bool cornerClear = check.GetPixel(1, 1).Alpha == 0;
    bool centreDrawn = check.GetPixel(22, 22).Alpha > 200;

    if (!cornerClear || !centreDrawn)
    {
        Console.Error.WriteLine(
            $"FAIL: corner alpha {check.GetPixel(1, 1).Alpha} (want 0), " +
            $"centre alpha {check.GetPixel(22, 22).Alpha} (want >200).");

        return 1;
    }

    Console.WriteLine($"Rasteriser OK: 44x44, transparent background, subject drawn. {probe}");
    return 0;
}

var spellData = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Data", "DnDSpells.xml");
var iconDirectory = @"C:\ClassicUO\src\ClassicUO.Client\Data\SpellIcons";

// Both overridable, because the client lives in a sibling repository whose location is a local
// choice rather than something this tool can know.
if (args.Length > 0) spellData = args[0];
if (args.Length > 1) iconDirectory = args[1];

spellData = Path.GetFullPath(spellData);

if (!File.Exists(spellData))
{
    Console.Error.WriteLine($"No spell data at {spellData}");
    return 1;
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
        element.GetAttribute("level"),
        element.GetAttribute("kind"),
        element.GetAttribute("description")));
}

Console.WriteLine($"{pending.Count} of {total} spell(s) need art.");

if (pending.Count == 0)
{
    return 0;
}

// A bare client picks up ANTHROPIC_API_KEY, ANTHROPIC_AUTH_TOKEN, or an `ant auth login` profile,
// in that order - so there is nothing to configure here beyond having one of them.
AnthropicClient client;

try
{
    client = new AnthropicClient();
}
catch (Exception e)
{
    Console.Error.WriteLine($"Could not create a client: {e.Message}");
    Console.Error.WriteLine("Set ANTHROPIC_API_KEY, or run `ant auth login`.");
    return 1;
}

// Four at a time. Enough to keep the run to a sensible length without tripping rate limits on a
// low tier; raise it if your limits allow.
using var throttle = new SemaphoreSlim(4);

int drawn = 0, failed = 0;
object consoleLock = new();

var work = pending.Select(async spell =>
{
    await throttle.WaitAsync();

    try
    {
        string? svg = await AskForSvg(client, spell);

        if (svg == null)
        {
            Interlocked.Increment(ref failed);
            return;
        }

        if (!Rasterise(svg, spell.Path))
        {
            lock (consoleLock)
            {
                Console.Error.WriteLine($"  {spell.Name}: the SVG did not rasterise");
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

Console.WriteLine($"\nDrew {drawn} icon(s), {failed} failed.");
Console.WriteLine(failed > 0 ? "Re-run to retry the failures - finished icons are skipped." : "Done.");

return failed > 0 ? 1 : 0;

/// <summary>
/// Asks for one icon. Returns the SVG source, or null if the request was declined or produced
/// nothing usable.
/// </summary>
static async Task<string?> AskForSvg(AnthropicClient client, Spell spell)
{
    string prompt = spell.Describe();

    for (int attempt = 1; attempt <= 3; ++attempt)
    {
        try
        {
            var response = await client.Messages.Create(new MessageCreateParams
            {
                Model = Model,
                MaxTokens = 8000,

                // Icon drawing is routine generative work rather than a reasoning problem, so the
                // effort dial sits below the default. Raise it if the art comes out flat - it is
                // the knob worth turning first.
                OutputConfig = new OutputConfig { Effort = Effort.Medium },

                System = StyleGuide,
                Messages = [new() { Role = Role.User, Content = prompt }],
            });

            // Checked before the content is read: a declined request returns a normal response
            // whose content is empty, and indexing into it would throw rather than report.
            if (response.StopReason == "refusal")
            {
                Console.Error.WriteLine($"  {spell.Name}: declined ({response.StopDetails?.Category})");
                return null;
            }

            var text = new StringBuilder();

            foreach (var block in response.Content.Select(b => b.Value).OfType<TextBlock>())
            {
                text.Append(block.Text);
            }

            string? svg = ExtractSvg(text.ToString());

            if (svg != null)
            {
                return svg;
            }

            Console.Error.WriteLine($"  {spell.Name}: no SVG in the reply (attempt {attempt})");
        }
        catch (Exception e) when (IsFatal(e))
        {
            // A bad key fails the same way on every spell, so retrying it 3 times each is 687
            // doomed requests and a wall of identical errors. Stop the run instead.
            Console.Error.WriteLine($"\nAuthentication failed: {e.Message}");
            Console.Error.WriteLine("Set ANTHROPIC_API_KEY, or run `ant auth login`.");

            Environment.Exit(1);
            return null;
        }
        catch (Exception e) when (attempt < 3)
        {
            // Rate limits and transient server errors are already retried inside the SDK; this
            // catches what survives that, and backs off before trying again.
            Console.Error.WriteLine($"  {spell.Name}: {e.Message} (attempt {attempt})");
            await Task.Delay(TimeSpan.FromSeconds(3 * attempt));
        }
    }

    return null;
}

/// <summary>Credential problems, which no amount of retrying will fix.</summary>
static bool IsFatal(Exception e)
{
    return e is Anthropic.Exceptions.AnthropicUnauthorizedException
        or Anthropic.Exceptions.AnthropicForbiddenException;
}

/// <summary>
/// Pulls the SVG out of a reply. Tolerant of a stray fence or sentence, because one malformed
/// wrapper should not cost the icon.
/// </summary>
static string? ExtractSvg(string text)
{
    int start = text.IndexOf("<svg", StringComparison.OrdinalIgnoreCase);
    int end = text.LastIndexOf("</svg>", StringComparison.OrdinalIgnoreCase);

    if (start < 0 || end < start)
    {
        return null;
    }

    return text[start..(end + "</svg>".Length)];
}

/// <summary>
/// Renders SVG to a 44x44 PNG with transparency - the size and format the client's loader wants.
/// </summary>
static bool Rasterise(string svg, string path, bool explain = false)
{
    try
    {
        using var source = new SKSvg();

        if (source.FromSvg(svg) == null || source.Picture == null)
        {
            // Swallowing this silently is how a broken rasteriser looks exactly like a broken
            // prompt: no icon, no reason. The self-test asks for the reason.
            if (explain)
            {
                Console.Error.WriteLine("  the SVG parsed to no picture");
            }

            return false;
        }

        using var bitmap = new SKBitmap(44, 44, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var canvas = new SKCanvas(bitmap);

        canvas.Clear(SKColors.Transparent);

        // The SVG declares a 44x44 viewBox, but a model can return a different one - scaling from
        // the picture's own bounds means an icon drawn at 100x100 still lands correctly.
        var bounds = source.Picture.CullRect;

        if (bounds.Width > 0 && bounds.Height > 0)
        {
            canvas.Scale(44f / bounds.Width, 44f / bounds.Height);
            canvas.Translate(-bounds.Left, -bounds.Top);
        }

        canvas.DrawPicture(source.Picture);
        canvas.Flush();

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var file = File.OpenWrite(path);

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

/// <summary>One spell, and the art direction that can be derived from its own row.</summary>
record Spell(string Name, string Path, string School, string Level, string Kind, string Description)
{
    public string Describe()
    {
        var brief = new StringBuilder();

        brief.Append($"Draw the icon for the D&D spell \"{Name}\".");

        // The description is written for a player, which makes it better art direction than
        // anything derivable from the numbers.
        if (!string.IsNullOrEmpty(Description))
        {
            brief.Append($" {Description.TrimEnd('.')}.");
        }

        brief.Append($" It is a level {Level} {School} spell.");

        string? element = Element();

        if (element != null)
        {
            brief.Append($" Render it in {element}.");
        }

        brief.Append(' ').Append(Mood());

        return brief.ToString();
    }

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

    private string Mood() => School switch
    {
        "Evocation" => "Show raw destructive energy.",
        "Abjuration" => "Show a protective ward or barrier.",
        "Conjuration" => "Show something summoned into being.",
        "Divination" => "Show an eye, a sigil, or revealed knowledge.",
        "Enchantment" => "Show an influence over another's will.",
        "Illusion" => "Show something half-real and shifting.",
        "Necromancy" => "Make it grim, touching on death.",
        "Transmutation" => "Show matter or form being changed.",
        _ => string.Empty,
    };
}
