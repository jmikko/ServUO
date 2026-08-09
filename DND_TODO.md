# TODO

Things deliberately not built yet, and what each is waiting on. Kept separate from
`DND_STATUS.md` so that "what exists" and "what is deferred and why" do not blur together.

---

## Resolved

The four structural blockers this file was organised around are gone. Recorded here because the
reasoning matters more than the fact:

**The turn economy.** This file used to say the choice between "a faithful turn-based port" and "a
real-time game with D&D's numbers" was a real design decision that should be made deliberately. It
was made: real-time. A turn-based port would mean discarding the swing timer, movement and spawning
that everything else is built on, to gain a pause between swings nobody asked for. So a turn is a
six-second window that refills on its own — one action, one bonus action, one reaction. The SRD's
ordering is lost; you cannot hold a reaction for a named trigger. The scarcity is kept, and scarcity
is what those features are about. Uncanny Dodge mattering once per round is most of Uncanny Dodge.

**Resource pools.** Ki, sorcery points and Lay on Hands now spend points rather than uses. The
distinction was the whole blocker: a use is all-or-nothing, so modelling ki as uses would have made
every ki ability cost the same regardless of what it was.

**Level-up pick-lists.** Fighting styles, expertise, invocations, pact boons and metamagic all
wanted the same thing — a list to choose from and a record of what was chosen — and building five
mechanisms for that would have been five places to get the entitlement arithmetic wrong. Fighting
styles in particular had been written and deliberately left unattached, because granting one
automatically raised every martial character's armour class.

**Creature-form substitution.** Wild Shape and the Polymorph family. The monster data was already
data-driven, which was most of what this needed; what was missing was a reliable swap back. The
beast's hit points are a separate pool, so a Druid knocked out of the form returns with their own
total intact — which is what makes Wild Shape defensive rather than cosmetic.

All twelve subclasses now have features of their own. Most of them were waiting on one of the four
above.

---

## Blocked on a client UI

Everything below works and has no way to be operated except a command. That is fine for testing and
poor for playing.

- **The level-up window does not collect choices.** `[choose` does. The window already collects a
  class, ability improvements and spells known, so this is a fifth list rather than a new mechanism.
- **Pools and turn resources are invisible.** `[points` reads them. A Monk should be able to see
  their ki without typing, and whether their reaction is spent.
- **Wild Shape forms are a list in chat.** `[use Wild Shape wolf`.
- **"How many?" is never asked.** Lay on Hands spends exactly what the wound needs and Divine Smite
  takes the highest slot available, because there is no way to prompt. Both should ask.

---

## Blocked on systems that do not exist at all

Not near-term work. Each is its own feature, and naming them is more honest than listing the spells
that need them.

- **Illusion and invisibility.** Minor Illusion, Mirror Image's actual mechanism, Greater Invisibility,
  the Cloak and Ring of Invisibility, Boots and Cloak of Elvenkind, the Hat of Disguise.
- **Planar travel.** Plane Shift, Gate, Etherealness, the Amulet of the Planes, the Well of Many Worlds.
- **Divination.** Scrying, True Seeing, Foresight, the Gem of Seeing.
- **Charges and activated items.** Potions, wands, the Deck of Many Things, the Horn of Valhalla.
  The wondrous table covers standing bonuses only, which is why it is a table; a dozen rows are
  currently inert and the self-test names each one at boot. `DnDMagicWeapon.OnHit` and
  `DnDMagicArmor.OnTakeDamage` are the seams for the ones that trigger in combat rather than on
  use - both are called by the resolver and both are currently empty everywhere.
- **The rest of the DMG item tables.** Only the standing-bonus items are in. Importing the rest is
  a real job, not a bulk paste: an item needs a layer that means something, art, a cost, and either
  bonuses in the table or code behind one of the hooks above. A row with a name and nothing else is
  worse than an absent row, because it looks finished. Weapons and armour in particular belong in
  `DnDWeapons.xml`/`DnDArmor.xml`, not the wondrous table - the wondrous table has no damage dice,
  and a "+2 chain mail" that lands on the talisman layer stacks its bonus on top of real armour.
- **Wish.** Its own category, deliberately.

---

## Approximations, recorded rather than hidden

These work, but not the way the SRD writes them. Listed so nobody has to rediscover the gap.

- **Great Weapon Fighting rerolls a total, not individual dice.** The resolver rolls a dice
  expression and gets back one number, so the "reroll 1s and 2s" rule is applied as "reroll a total
  in the bottom fifth of the range". Close in expectation, not the same rule.
- **Flurry of Blows grants advantage rather than two extra strikes.** Resolving the strikes here
  would duplicate the combat resolver's damage rules in a second place.
- **Cunning Action's Dash and Disengage have no meaning** without turn-based movement, so it does
  the third thing — breaking off — and grants advantage on the next attack.
- **Colossus Slayer rides the bonus action** to get its once-per-round limit, because a passive
  feature has no other per-round currency to claim.
- **Only Riposte of the Battle Master manoeuvres exists.** It is the one whose trigger the engine
  can see. The rest want superiority dice, which is another resource pool, and a way to declare
  intent before a roll — which the real-time model does not have.
- **Metamagic is two options of six.** Careful, Distant, Subtle and Twinned change who or how a
  spell reaches, which needs a targeting rework. Empowered and Quickened are about the roll.
