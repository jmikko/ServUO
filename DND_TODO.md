# TODO

Things deliberately not built yet, and what each is waiting on. Kept separate from
`DND_STATUS.md` so that "what exists" and "what is deferred and why" do not blur together.

---

## Blocked on a turn economy

This is a real-time server. There are no turns, no actions, no bonus actions and no reactions —
combat resolves on a swing timer. Every feature below is phrased in the SRD as something you do
*instead of* or *in reaction to* something else, and none of them has anywhere to live until
there is a notion of a turn to spend.

Implementing them anyway would produce features that fire at arbitrary moments and cannot be
tested, which is worse than their absence.

- **Cunning Action** (Rogue 2) — dash, disengage or hide as a bonus action
- **Uncanny Dodge** (Rogue 5) — halve one attack's damage, as a reaction
- **Evasion** (Rogue 7, Monk 7) — no damage on a successful Dexterity save
- **Deflect Missiles** (Monk 3) — reduce ranged damage, as a reaction
- **Riposte, Parry and the Battle Master manoeuvres**
- **Opportunity attacks** generally

**What would unblock them:** a per-combatant turn state with an action, a bonus action and a
reaction, each refreshing on the swing timer. That is a real design decision about what this game
is — a faithful turn-based port, or a real-time game with D&D's numbers — and it should be made
deliberately rather than fallen into.

---

## Blocked on a resource-pool system

Several classes spend from a pool of points, choosing how many to spend per use. The activated
feature system added alongside class features handles *uses*, not *points*, and the difference
matters: a Monk spending 2 ki of 5 on one ability is a different mechanism from spending one of
three uses.

- **Ki** (Monk 2) — Flurry of Blows, Patient Defense, Step of the Wind, Stunning Strike
- **Sorcery Points and Metamagic** (Sorcerer 2, 3)
- **Divine Smite** (Paladin 2) — spends a spell slot of a chosen level on extra damage
- **Lay on Hands** as written — a pool of hit points, currently a fixed 5 per use

**What would unblock them:** a point pool per class with a spend-N interface, plus a way for the
client to ask "how many?" at the moment of use.

---

## Blocked on a pick-list at level-up

The level-up window already collects a class, ability improvements and spells known. These need
the same treatment: a list to choose from, stored on the character.

- **Fighting Styles** (Fighter 1, Paladin 2, Ranger 2) — the features exist in
  `Features/PassiveFeatures.cs` and are deliberately unattached. Granting one automatically was
  tried and reverted: it silently raised every martial character's armour class.
- **Eldritch Invocations** (Warlock 2)
- **Pact Boon** (Warlock 3)
- **Metamagic options** (Sorcerer 3)
- **Expertise** (Rogue 1, Bard 3) — doubles proficiency on chosen skills

---

## Blocked on creature-form substitution

- **Wild Shape** (Druid 2), **Polymorph**, **Shapechange**, **True Polymorph**

A character would have to adopt another creature's stat block while keeping their own identity,
hit points and equipment state. The monster data is already data-driven, which is most of what
this needs, but nothing can currently swap a player's stats for a monster's and back.

---

## Subclass features

All twelve subclasses exist and resolve their parent correctly, but only the Champion has
features of its own (Improved Critical). The other eleven need the systems above more often than
not — Circle of the Land needs Wild Shape, the Fiend needs invocations, Way of the Open Hand
needs ki.

---

## Smaller, unblocked

These need no new system and are simply not done:

- The 60 spells registered with no mechanical effect (see `DND_STATUS.md`)
- Magic item catalogue beyond the four types that exist
- Feats beyond Tough

Death saves and hit dice are done. Two things they left behind:

- **Dying has no client UI.** The successes and failures arrive as system messages. They want to be
  three pips somewhere visible, since the whole tension of the rule is watching the count.
- **What happens after death is still UO's.** Three failed saves ends in a UO ghost and a healer.
  Revivify, and what resurrection costs, are a separate question from how you get there.
