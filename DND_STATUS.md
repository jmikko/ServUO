# D&D 5.5e conversion — status

ServUO rebuilt as a D&D 5.5e server. Not D&D layered on top of Ultima Online: UO's rules are
being replaced, and UO content with no D&D equivalent is deleted rather than adapted.

- **Server** — this repo, branch `dnd-additive` (fork remote: `jmikko/ServUO`)
- **Client** — `C:\ClassicUO`, branch `dnd-client` (fork remote: `jmikko/ClassicUO`)

Both branches track their `fork/` counterpart, so a plain `git push` goes to the fork. `origin`
still points at upstream in both, so upstream changes can be pulled without extra setup.

---

## How this is built

The conversion is **additive**. An earlier attempt tried to delete UO content until only the
D&D parts remained; it did not converge, because ServUO's Scripts layer is a dense web — vendors
reference items, decoration references addons, loot references monsters — so every deletion cut
many edges at once. Error counts oscillated in the thousands without ever trending down.

So instead: the original 4,000-file Scripts tree sits parked in `Scripts/Legacy/`, excluded from
compilation, and files move **out** of it only when something demonstrably needs them. The live
tree is 99 files.

The rule that keeps it converging: **never port a core ServUO type that drags its subsystems in
behind it — write a lightweight replacement against `Server/` instead.** `DnDCreature` replaces
`BaseCreature`, `DnDPlayerMobile` replaces a 12,000-line `PlayerMobile`, one file each.

`Server/` (the engine) cannot see `Scripts/` (the content). Where the engine needs to know about
D&D state, it goes through an interface defined in `Server/` and implemented in `Scripts/` —
`IDnDCharacter`, `IDnDCreature`, `IDnDEquipment`, `IDnDSpecies`. Rules that the engine itself has
to consult on every roll — conditions, roll modifiers — live in `Server/` outright.

Bulk content is **data, not classes**. Monsters, weapons, armour and spells are rows in XML;
adding one is a row plus, where reflection needs it, a six-line stub.

---

## What works

| System | State |
|---|---|
| d20 combat | attack rolls, AC, criticals, damage dice, finesse, versatile, reach |
| Character creation | species + class + ability scores, over the wire, from the real client |
| 12 classes | all SRD, with hit dice, saves, proficiencies, spellcasting progression |
| 8 species | all SRD: Human, Elf, Dwarf, Halfling, Gnome, Half-Orc, Tiefling, Dragonborn |
| Advancement | XP, levels 1–20, HP growth, proficiency scaling, long and short rests |
| Spellcasting | slots, save DCs, spell attacks, upcasting, cantrip scaling, concentration |
| 174 spells | cantrips through 9th level |
| 37 weapons, 13 armour | the complete SRD tables |
| 29 monsters | SRD stat blocks, spawning in the world |
| 14 conditions | feeding advantage and disadvantage into every roll |
| Area shapes | cones, lines, spheres, cubes with real geometry |
| Client | character setup, character sheet, spellbook, level-up gump |
| 18 skills | all SRD, with ability checks and skill checks that add proficiency |
| Multiclassing | levels tracked per class; SRD multiclass slot table |
| Advancement choices | level-up is a player decision: which class, ability improvements, spells learnt |
| Magic items | attunement, capped at three, with attack/damage/save bonuses |

Verified end to end in the actual game client: log in, create a character, pick a species and
class, receive a spellbook, click a spell, target something, and have the server resolve it.

### Two rules that are easy to get wrong

Multiclassing splits what used to be one number, and the two halves are not interchangeable:

- **Proficiency bonus comes from `TotalLevel`** — levels across every class. Deriving it from a
  single class would make multiclassing a way to farm proficiency.
- **Saving throw proficiencies come from `PrimaryClass`** — the class you started as, and only
  that one. A dictionary has no first entry, which is why `PrimaryClass` exists on the interface
  at all.

**Levelling is two steps.** Experience grants *pending* levels; choosing which class each one goes
into is what multiclassing is. A character whose player never answers the prompt stays at their old
level — which means **a client build without `DnDLevelUpGump` cannot level up at all.** Republish
the client before testing advancement.

### What is deliberately gone

UO systems with no D&D equivalent have been removed rather than left dormant: mana and spell
circles, skill-based combat, Fame and Karma, the Gargoyle race, quests, champion spawns, treasure
maps, pet training. Monsters carry a **challenge rating** instead of Fame/Karma, because CR is the
only difficulty measure a D&D monster has and it is what the XP award derives from.

---

## Honest gaps

**60 of the 174 spells have no mechanical effect.** They are registered so they appear on spell
lists and cost slots, and each says *"Not yet modelled"* in its own description rather than
silently doing nothing. What remains needs systems well outside combat — Polymorph, Animate Dead,
True Resurrection, Wish — or fine-grained targeting the data cannot yet express.

**No class features at all.** This is the largest gap. A class is currently a hit die, a set of
proficiencies and a spell list — so a Fighter and a Barbarian play identically, and the twelve
subclasses rename a character without changing anything. Nothing implements Extra Attack, Sneak
Attack, Rage, Second Wind, Action Surge or any subclass feature. Extra Attack alone is most of
what separates a martial class at 5th level from the same class at 1st.

**Death is still Ultima Online's.** At 0 hit points a character becomes a UO ghost. D&D has
unconsciousness, death saving throws, stabilising, and dying at three failures — none of it
exists, and it is a pillar of how the game feels in play.

**One feat.** Tough. SRD has roughly forty.

**Magic items are four types.** Ring of Protection, Amulet of Health, and abstract magic
weapon/armour bases. Attunement and bonus stacking work; the catalogue does not exist.

**Hit dice spending on a short rest is unmodelled.**

**Some spell tactics are approximated.** Damage numbers are SRD-accurate, but Scorching Ray rolls
6d6 as one lump rather than three separately-aimed rays, and Chain Lightning is a sphere rather
than a primary target with arcs.

**Tools, adventuring gear and magic items are absent.** Mostly inert without crafting, skills and
attunement to give them meaning.

**Only Fireball has been cast above 1st level**, and that server-side. Nothing from levels 2–9 has
been exercised from the real client.

---

## Next steps

In the order I would take them:

1. **Class features.** The biggest gap by a distance, and the thing that makes classes feel like
   classes. Needs a feature system first — something that hangs passive and triggered effects off
   a class and level, and that combat consults — then Extra Attack, Sneak Attack, Rage, Second
   Wind. The twelve subclasses are waiting on exactly this to become more than a name.
2. **Death and dying.** Unconsciousness at 0 hit points, death saving throws, stabilising. Small,
   self-contained, and removes the most jarring surviving piece of Ultima Online.
3. **Magic item catalogue.** Attunement and stacking already work, so this is mostly rows.
4. **Feats.** The mechanism works; forty of them do not exist.
5. **Client polish**, hit dice on a short rest, and the remaining spell placeholders.

---

## Working on this

### Building and deploying

Both repos silently run stale binaries if you only build. **What you build is not what runs.**

```bash
cd /c/ServUO && dotnet build Server/Server.csproj -c Debug && dotnet build Scripts/Scripts.csproj -c Debug
cp Server/obj/Debug/ServUO.exe ServUO.exe && cp Scripts/bin/Debug/Scripts.dll Scripts.dll
```

`Server/ScriptCompiler.cs` loads `Scripts.dll` from the repo root, but the project builds to
`Scripts/bin/Debug/`. `Server.csproj` declares `<OutputPath>..\</OutputPath>` but MSBuild routinely
skips that copy and still reports success. Check timestamps rather than trusting "Build succeeded".
A running server also locks both files, so stop it first.

The client is NativeAOT and has the same problem in a different shape:

```bash
cd /c/ClassicUO && dotnet publish src/ClassicUO.Client/ClassicUO.Client.csproj -c Release -r win-x64 --self-contained -o bin/dist
```

Then launch `bin/dist/cuo.exe` directly. The `ClassicUO.exe` launcher loads `cuo.dll`, which is an
older copy of the same native image under a different name. To confirm a build contains a change:
`grep -c "SomeNewTypeName" bin/dist/cuo.exe`.

### The self-test

`Scripts/Misc/DnDCombatSelfTest.cs` runs at boot when `Diagnostics.CombatSelfTest=True` in
`Config/Diagnostics.cfg`. It is off by default because it spawns and deletes mobiles in the live
world. It checks the rules statistically rather than by assertion — 4,000 swings against an
expected hit rate — because a resolver that always hits, never hits, or ignores AC passes a
"did something happen" test perfectly well.

It has caught real bugs repeatedly: half-casters rounding the wrong way, lines three tiles wide
instead of one, `Movement.Impl` unset so nothing could walk.

**Engine hooks that default to null are the recurring trap on this codebase.** Three have bitten:
`Mobile.DefaultWeapon` (combat timer threw), `Mobile.FatigueHandler` (every damage application
threw), `Movement.Impl` (nothing could move). Each was assigned only by a file now parked in
`Legacy/`, and each failed silently. A sweep found the rest are null-guarded, but new ones will
appear as more of `Server/` gets exercised — the self-test asserts the ones that matter.

### Configuration

| File | Key | Purpose |
|---|---|---|
| `Config/Diagnostics.cfg` | `CombatSelfTest` | run the boot self-test |
| `Config/Startup.cfg` | `Location`, `Map` | where new characters appear |
| `Config/Spawning.cfg` | `AutoStartRegionSpawns` | fill spawners at boot |
| `Config/Expansion.cfg` | `CurrentExpansion` | gates which species the client has art for |

### Player commands

`[sheet` `[spells` `[rest` `[shortrest`

These exist because the client has no D&D UI beyond creation, the sheet and the spellbook.
