# IconForge

Draws a spell icon for every spell that does not have one.

## What it actually does

**Claude cannot produce a raster image.** There is no API that returns a PNG, so nothing can ask
for one directly. What Claude *can* do is write SVG — which is text, and which rasterises to a PNG
cleanly at any size. So this asks for vector art and converts it here.

The result is stylised rather than painted: crisp shapes, gradients, and glow, closer to a game
icon than to an illustration. If you want painted art like a hand-made `Fireball.png`, this tool is
not that — make those by hand and IconForge will leave them alone.

## Running it

Needs credentials. Either:

```bash
setx ANTHROPIC_API_KEY sk-ant-...
```

or `ant auth login`, which stores a profile the SDK reads automatically.

Then:

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

## Checking the machinery without spending anything

```bash
dotnet run -c Release --project Tools/IconForge -- --selftest
```

This exercises the half that needs no API key: SVG in, 44×44 PNG out, transparency intact. It is
worth having separately because a broken rasteriser and a broken prompt look identical from the
outside — both produce no icon — and only one of them costs money to diagnose. It caught a real
SkiaSharp native/managed version mismatch when this tool was written.

## Tuning

Everything worth changing is at the top of `Program.cs`:

- **`StyleGuide`** — the house style, shared by every icon. Consistency across the set matters more
  than any single icon: 229 icons that agree read as a game, 229 that do not read as a clip-art
  folder. Change this and delete the icons you want redrawn.
- **`OutputConfig.Effort`** — set to `Medium`, because icon drawing is routine generative work
  rather than a reasoning problem. **This is the first knob to turn if the art comes out flat.**
- **`SemaphoreSlim(4)`** — four requests in flight. Raise it if your rate limits allow.

Per-spell art direction is derived from the spell's own row: its description (written for a player,
so better direction than anything from the numbers), plus a colour taken from what the spell *does*
rather than its school — a player recognises a fire spell by it being orange, not by it being
Evocation.

## Cost

One request per spell, 229 spells. If you want to halve that, the Batches API runs the same
requests asynchronously at 50% — worth doing for a full regeneration, not worth the polling
complexity for topping up a handful.

## Failures

Failures are reported per spell and counted at the end; re-running retries only those, since the
successes now have files. Authentication failures stop the whole run immediately rather than
retrying 229 times.
