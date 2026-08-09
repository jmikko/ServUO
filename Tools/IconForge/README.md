# IconForge

Paints a spell icon for every spell that does not have one.

## What it uses, and why not Claude

This calls **Google's Gemini image models**, not Claude. The Anthropic API has no image generation
at all — there is no endpoint that returns a picture — so painted art has to come from somewhere
else.

An earlier version had Claude write SVG, which this tool rasterised. That worked and produced clean
vector tiles, but never looked *painted*: hand-written vector paths have a hard ceiling against
real brushwork, and the results read as flat and cartoonish next to reference art from Baldur's
Gate or Pathfinder. Generating raster art directly clears that ceiling.

## Running it

Get a key from [Google AI Studio](https://aistudio.google.com/apikey) — there is a free tier, which
is the cheap way to iterate on style. Then:

```bash
setx GEMINI_API_KEY your-key-here
```

Open a **new** terminal (`setx` only affects new processes), then draw a few:

```bash
dotnet run -c Release --project Tools/IconForge -- --limit 6
```

Look at those before committing to the rest. When happy:

```bash
dotnet run -c Release --project Tools/IconForge
```

It reads `Data/DnDSpells.xml` and writes into
`C:\ClassicUO\src\ClassicUO.Client\Data\SpellIcons`. Override both if your layout differs:

```bash
dotnet run -c Release --project Tools/IconForge -- ../../Data/DnDSpells.xml D:/path/to/SpellIcons
```

## It is resumable, and it never overwrites

Any spell that already has a PNG is skipped. That single rule gives you three things:

- An interrupted run continues where it stopped — just run it again.
- A single icon can be redrawn by deleting its file.
- **Hand-drawn art is never overwritten.** Your own `Fireball.png` survives every run.

Combined with `--limit`, that is the whole iteration loop: draw six, look, delete the ones you
dislike, adjust the style, run again.

## Checking the machinery without spending anything

```bash
dotnet run -c Release --project Tools/IconForge -- --selftest
```

This exercises the half that needs no API key: image bytes in, 44×44 PNG out, content intact. It is
worth having separately because a broken converter and a broken prompt look identical from the
outside — both produce no icon — and only one of them costs money to diagnose. The SVG-era version
of this test immediately caught a SkiaSharp native/managed version mismatch.

## The size constraint drives everything

Icons display at **44×44**. That is small, and it is the single most important fact about the
prompt.

Generation happens at 512px (`ImageSize`) and downscales here. Asking for a larger source would
only throw away more pixels — it costs more and does not improve the result. More importantly, a
*busy* 512px painting turns to mush at 44px, which is why the style guide insists on a bold
silhouette, few elements, and high contrast, even while asking for painterly rendering. That is
also why the reference art it imitates looks the way it does.

The downscale uses Mitchell cubic resampling. Going 512 → 44 discards 99% of the pixels, and a
naive nearest-neighbour pick would produce aliased noise from a painting that looked fine at full
size.

## Tuning

Everything worth changing is at the top of `Program.cs`, plus the `Style` class at the foot:

- **`Style.Guide`** — the house style, shared by every icon. Consistency across the set matters
  more than any single icon: 229 icons that agree read as a game, 229 that do not read as a
  clip-art folder.
- **`Model`** — `gemini-3.1-flash-image` by default. `gemini-3.1-flash-lite-image` is cheaper and
  faster (1K only, which the downscale does not mind); `gemini-3-pro-image` is the premium tier.
- **`SemaphoreSlim(3)`** — three requests in flight. Image generation is slower and more
  rate-limited than text, and free tiers are tighter still. Raise it once you know your quota holds.

Per-spell direction is derived from the spell's own row: its description (written for a player, so
better direction than anything from the numbers), a subject implied by its school, the school's
palette for the tile and border, and a colour taken from what the spell *does* rather than its
school — a player recognises a fire spell by it being orange, not by it being Evocation.

## Failures

Failures are reported per spell and counted at the end; re-running retries only those, since the
successes now have files. Authentication failures stop the whole run immediately rather than
retrying 229 times.

The response parser looks for the longest base64 string in the reply rather than walking a fixed
path, because this API's response shape has changed before and a tool that breaks on a renamed
wrapper field is a tool that breaks again.
