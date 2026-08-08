using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells.DnD;
using Server.Regions;

namespace Server.Misc
{
	/// <summary>
	/// Boot-time smoke test for D&amp;D combat, run against real world-resident mobiles so it also
	/// catches the wiring faults a pure test of the dice math would sail straight past: a null
	/// <see cref="Mobile.DefaultWeapon"/> (which makes the engine's combat timer throw on the first
	/// swing), or damage that resolves but never lands on the target.
	/// <para>
	/// Off unless <c>Diagnostics.CombatSelfTest</c> is true. It spawns and then deletes two mobiles
	/// in the live world, so it has no business running on a real shard.
	/// </para>
	/// </summary>
	public static class DnDCombatSelfTest
	{
		private static readonly bool Enabled = Config.Get("Diagnostics.CombatSelfTest", false);

		// Inside the SRD Test Grounds, on the Despise entrance tile.
		private static readonly Point3D TestLocation = new Point3D(5501, 570, 59);

		public static void Initialize()
		{
			if (Enabled)
			{
				EventSink.ServerStarted += OnServerStarted;
			}
		}

		private static void OnServerStarted()
		{
			try
			{
				Run();
			}
			catch (Exception e)
			{
				Console.WriteLine("[combat-selftest] FAILED with exception: {0}", e);
			}
		}

		private static void Run()
		{
			Console.WriteLine("[combat-selftest] starting");

			SrdGoblin goblin = new SrdGoblin();

			DnDPlayerMobile fighter = new DnDPlayerMobile
			{
				Name = "CombatSelfTest",
				Body = 0x190
			};

			fighter.ApplyDnDSetup(
				new AbilityScores(16, 12, 14, 10, 10, 8),
				CharacterClass.Parse("Fighter"));

			fighter.MoveToWorld(TestLocation, Map.Felucca);
			goblin.MoveToWorld(TestLocation, Map.Felucca);

			ReportStatBlock(fighter, goblin);

			bool ok = true;

			ok &= CheckSpawnAnchors();
			ok &= CheckWeaponResolves(fighter);
			ok &= CheckWeaponResolves(goblin);
			ok &= CheckFinesseAndProficiency();

			// Unarmed: 1d1 + Str mod.
			ok &= RunSwings("fighter unarmed", fighter, goblin, null, 1, 1, 0, 3);

			DnDLongsword sword = new DnDLongsword();
			fighter.EquipItem(sword);

			ok &= CheckWeaponResolves(fighter);

			// Armed: the wielded weapon must be what Mobile.Weapon returns, and its dice - not the
			// unarmed 1d1 - must be what damage comes from.
			ok &= RunSwings("fighter w/ longsword", fighter, goblin, sword, 1, 8, 0, 3);

			// The creature's own attack: damage comes from its stat block, with no ability modifier
			// added on top (the stat block already bakes one in).
			ok &= RunSwings("goblin", goblin, fighter, null, 1, 6, 2, 0);

			ok &= CheckDamageIsApplied(fighter, goblin, sword);
			ok &= CheckSpellSlotTables();
			ok &= CheckSpellcasting(fighter);
			ok &= CheckAdvancement();

			fighter.Delete();
			goblin.Delete();

			Console.WriteLine(ok ? "[combat-selftest] PASS" : "[combat-selftest] FAIL");
		}

		private static void ReportStatBlock(DnDPlayerMobile fighter, SrdGoblin goblin)
		{
			Console.WriteLine(
				"[combat-selftest]   fighter: AC {0}  HP {1}  attack +{2}",
				fighter.ArmorClass,
				fighter.HitsMax,
				CombatRules.GetAttackBonus(fighter, false));

			Console.WriteLine(
				"[combat-selftest]   goblin:  AC {0}  HP {1}  attack +{2}  damage {3}",
				goblin.ArmorClass,
				goblin.HitPointsMaxDnD,
				goblin.AttackBonus,
				goblin.DamageDiceExpression);
		}

		/// <summary>
		/// A mobile with nothing equipped must still return a weapon; the engine's combat timer
		/// dereferences it without a null check.
		/// </summary>
		private static bool CheckWeaponResolves(Mobile m)
		{
			IWeapon weapon = m.Weapon;

			if (weapon == null)
			{
				Console.WriteLine("[combat-selftest] FAIL: {0} has no weapon (Mobile.DefaultWeapon unset?)", m.Name);
				return false;
			}

			Console.WriteLine("[combat-selftest]   {0} weapon: {1} (range {2})", m.Name, weapon.GetType().Name, weapon.MaxRange);
			return true;
		}

		/// <summary>
		/// A region entry can explicitly carry an anchor/range; otherwise a D&amp;D creature falls back
		/// to the random tile it spawned on and its default wander range. Both paths must be valid.
		/// </summary>
		private static bool CheckSpawnAnchors()
		{
			int checkedCount = 0;
			int mismatches = 0;

			foreach (Mobile mobile in World.Mobiles.Values)
			{
				DnDCreature creature = mobile as DnDCreature;
				SpawnEntry entry = creature == null ? null : creature.Spawner as SpawnEntry;

				if (entry == null)
				{
					continue;
				}

				++checkedCount;

				bool hasConfiguredHome = entry.HomeLocation != Point3D.Zero;
				bool matches = hasConfiguredHome
					? creature.Home == entry.HomeLocation && creature.RangeHome == entry.HomeRange
					: creature.Home != Point3D.Zero && creature.RangeHome > 0;

				if (!matches)
				{
					++mismatches;
				}
			}

			Console.WriteLine(
				"[combat-selftest]   spawn anchors: {0} checked, {1} mismatch(es)",
				checkedCount,
				mismatches);

			return checkedCount > 0 && mismatches == 0;
		}

		/// <summary>
		/// A low-Strength, high-Dexterity wizard must use Dexterity with a dagger, while the same
		/// character must be refused medium armour. These are the two seams that keep the compact
		/// equipment layer honest as more SRD items are added.
		/// </summary>
		private static bool CheckFinesseAndProficiency()
		{
			DnDPlayerMobile wizard = new DnDPlayerMobile
			{
				Name = "EquipmentSelfTest",
				Body = 0x190
			};

			wizard.ApplyDnDSetup(
				new AbilityScores(8, 16, 12, 16, 10, 10),
				CharacterClass.Parse("Wizard"));

			DnDDagger dagger = new DnDDagger();
			DnDChainShirt chainShirt = new DnDChainShirt();

			int attackBonus = CombatRules.GetAttackBonus(wizard, false, dagger.IsFinesse);
			int damageBonus = CombatRules.GetDamageBonus(wizard, false, dagger.IsFinesse);
			bool blockedArmor = !wizard.EquipItem(chainShirt);

			dagger.Delete();
			chainShirt.Delete();
			wizard.Delete();

			Console.WriteLine(
				"[combat-selftest]   dagger finesse: attack +{0}, damage +{1}; wizard chain shirt blocked: {2}",
				attackBonus,
				damageBonus,
				blockedArmor);

			return attackBonus == 5 && damageBonus == 3 && blockedArmor;
		}

		/// <summary>
		/// Swings the resolver enough times to make the hit rate meaningful, then checks it against
		/// the rate the d20 math predicts. With a wide tolerance this catches a resolver that always
		/// hits, never hits, or ignores AC, without being flaky.
		/// <para>
		/// Statistics come from the pure roll rather than from watching the target's HP: a target
		/// that dies (or is Blessed, or is at full health already) makes an HP delta a liar, and
		/// the whole point here is to check the dice, not the bookkeeping. That damage genuinely
		/// reaches the target is checked separately, by <see cref="CheckDamageIsApplied"/>.
		/// </summary>
		private static bool RunSwings(
			string label,
			Mobile attacker,
			Mobile defender,
			IDnDEquipment weapon,
			int diceCount,
			int diceSides,
			int diceBonus,
			int abilityBonus)
		{
			const int Swings = 4000;

			int hits = 0, totalDamage = 0, lowest = int.MaxValue, highest = 0;

			for (int i = 0; i < Swings; i++)
			{
				DnDCombat.AttackResult result = DnDCombat.RollAttack(attacker, defender, weapon);

				if (!result.Hit)
				{
					continue;
				}

				++hits;
				totalDamage += result.Damage;
				lowest = Math.Min(lowest, result.Damage);
				highest = Math.Max(highest, result.Damage);
			}

			double actualRate = hits / (double)Swings;

			// A natural 1 always misses and a natural 20 always hits, so of the 20 faces the ones
			// that land are 20 itself plus every face from `needed` to 19.
			int needed = CombatRules.GetArmorClass(defender) - CombatRules.GetAttackBonus(attacker, false);
			double expectedRate = Math.Min(19, Math.Max(1, 21 - needed)) / 20.0;

			double avgDamage = hits > 0 ? totalDamage / (double)hits : 0.0;

			// A crit rolls the whole damage expression a second time, so any bonus written into the
			// expression itself (a monster's "1d6+2") is doubled, while the attacker's ability
			// modifier - added afterwards - is not.
			double avgDice = (diceCount * (diceSides + 1) / 2.0) + diceBonus;
			double expectedAvg = avgDice + abilityBonus + (0.05 / expectedRate * avgDice);

			int expectedLow = Math.Max(1, diceCount + diceBonus + abilityBonus);
			int expectedHigh = (((diceCount * diceSides) + diceBonus) * 2) + abilityBonus; // max-roll crit

			Console.WriteLine(
				"[combat-selftest]   {0}: {1}/{2} hits ({3:P1}, expected {4:P1}); damage {5}-{6} avg {7:F2} (expected {8}-{9} avg {10:F2})",
				label,
				hits,
				Swings,
				actualRate,
				expectedRate,
				lowest == int.MaxValue ? 0 : lowest,
				highest,
				avgDamage,
				expectedLow,
				expectedHigh,
				expectedAvg);

			if (Math.Abs(actualRate - expectedRate) > 0.04)
			{
				Console.WriteLine("[combat-selftest] FAIL: {0} hit rate is not consistent with d20 vs AC", label);
				return false;
			}

			if (hits == 0)
			{
				Console.WriteLine("[combat-selftest] FAIL: {0} never landed a hit", label);
				return false;
			}

			if (lowest < expectedLow || highest > expectedHigh)
			{
				Console.WriteLine("[combat-selftest] FAIL: {0} damage fell outside the weapon's dice range", label);
				return false;
			}

			if (Math.Abs(avgDamage - expectedAvg) > 0.35)
			{
				Console.WriteLine("[combat-selftest] FAIL: {0} average damage does not match the expected dice", label);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Rolls until something lands, then confirms the damage actually left the resolver and
		/// reached the target's hit points - the seam Mobile.Damage sits on.
		/// </summary>
		private static bool CheckDamageIsApplied(Mobile attacker, Mobile defender, IDnDEquipment weapon)
		{
			for (int i = 0; i < 200; i++)
			{
				defender.Hits = defender.HitsMax;

				int before = defender.Hits;

				DnDCombat.Resolve(attacker, defender, weapon);

				if (defender.Hits < before)
				{
					Console.WriteLine(
						"[combat-selftest]   damage application: {0} HP {1} -> {2}",
						defender.Name,
						before,
						defender.Hits);

					defender.Hits = defender.HitsMax;
					return true;
				}

				if (!defender.Alive)
				{
					Console.WriteLine("[combat-selftest]   damage application: {0} was killed outright", defender.Name);
					return true;
				}
			}

			Console.WriteLine("[combat-selftest] FAIL: 200 swings never reduced the target's hit points");
			return false;
		}

		/// <summary>
		/// Spot-checks the SRD slot tables at the boundaries that are easy to get wrong: a
		/// half-caster having nothing at 1st, and Pact Magic putting every slot at one level.
		/// </summary>
		private static bool CheckSpellSlotTables()
		{
			bool ok = true;

			ok &= CheckSlots("Wizard 1", SpellProgression.Full, 1, 1, 2);
			ok &= CheckSlots("Wizard 1 (2nd-level)", SpellProgression.Full, 1, 2, 0);
			ok &= CheckSlots("Wizard 5 (3rd-level)", SpellProgression.Full, 5, 3, 2);
			ok &= CheckSlots("Wizard 20 (9th-level)", SpellProgression.Full, 20, 9, 1);

			// A Paladin casts nothing at 1st and as a 1st-level caster at 2nd.
			ok &= CheckSlots("Paladin 1", SpellProgression.Half, 1, 1, 0);
			ok &= CheckSlots("Paladin 2", SpellProgression.Half, 2, 1, 2);
			ok &= CheckSlots("Paladin 5 (2nd-level)", SpellProgression.Half, 5, 2, 2);

			// Pact Magic: all slots sit at one level, and none exist below it.
			ok &= CheckSlots("Warlock 1", SpellProgression.Pact, 1, 1, 1);
			ok &= CheckSlots("Warlock 3 (2nd-level)", SpellProgression.Pact, 3, 2, 2);
			ok &= CheckSlots("Warlock 3 (1st-level)", SpellProgression.Pact, 3, 1, 0);

			ok &= CheckSlots("Fighter 20", SpellProgression.None, 20, 1, 0);

			Console.WriteLine("[combat-selftest]   spell slot tables: {0}", ok ? "OK" : "MISMATCH");

			return ok;
		}

		private static bool CheckSlots(string label, SpellProgression progression, int level, int spellLevel, int expected)
		{
			int actual = Spellcasting.GetMaxSlots(progression, level, spellLevel);

			if (actual == expected)
			{
				return true;
			}

			Console.WriteLine("[combat-selftest] FAIL: {0} slots expected {1}, got {2}", label, expected, actual);
			return false;
		}

		/// <summary>
		/// Casts real spells through the real entry point, checking the things that are meant to
		/// refuse a cast actually refuse it, and that slots are spent exactly once.
		/// </summary>
		private static bool CheckSpellcasting(DnDPlayerMobile fighter)
		{
			bool ok = true;

			// Blessed, because a goblin has 7 hit points and Fire Bolt averages more than that: an
			// unblessed dummy dies on the first cantrip and every later cast reports NoTarget.
			// Damage actually landing is checked separately, against a target meant to die.
			SrdGoblin dummy = new SrdGoblin { Blessed = true };
			dummy.MoveToWorld(TestLocation, Map.Felucca);

			DnDPlayerMobile wizard = new DnDPlayerMobile { Name = "SelfTestWizard", Body = 0x190 };

			wizard.ApplyDnDSetup(
				new AbilityScores(8, 14, 12, 16, 10, 10),
				CharacterClass.Parse("Wizard"));

			wizard.MoveToWorld(TestLocation, Map.Felucca);

			// Int 16 -> +3, proficiency +2. SRD: DC = 8 + prof + mod, attack = prof + mod.
			ok &= CheckValue("wizard save DC", Spellcasting.GetSaveDC(wizard), 13);
			ok &= CheckValue("wizard spell attack", Spellcasting.GetSpellAttackBonus(wizard), 5);
			ok &= CheckValue("wizard 1st-level slots", wizard.GetAvailableSpellSlots(1), 2);

			DnDSpell fireBolt = SpellRegistry.Find("Fire Bolt");
			DnDSpell magicMissile = SpellRegistry.Find("Magic Missile");
			DnDSpell cureWounds = SpellRegistry.Find("Cure Wounds");

			if (fireBolt == null || magicMissile == null || cureWounds == null)
			{
				Console.WriteLine("[combat-selftest] FAIL: starter spells are not registered");
				wizard.Delete();
				dummy.Delete();
				return false;
			}

			// A cantrip costs nothing, however many times it is cast.
			ok &= CheckCast("cantrip", DnDCasting.Cast(wizard, fireBolt, dummy), CastResult.Success);
			ok &= CheckCast("cantrip again", DnDCasting.Cast(wizard, fireBolt, dummy), CastResult.Success);
			ok &= CheckValue("slots after 2 cantrips", wizard.GetAvailableSpellSlots(1), 2);

			// A levelled spell spends exactly one slot per cast, and stops when they run out.
			ok &= CheckCast("magic missile", DnDCasting.Cast(wizard, magicMissile, dummy), CastResult.Success);
			ok &= CheckValue("slots after 1 spell", wizard.GetAvailableSpellSlots(1), 1);

			ok &= CheckCast("magic missile 2", DnDCasting.Cast(wizard, magicMissile, dummy), CastResult.Success);
			ok &= CheckValue("slots after 2 spells", wizard.GetAvailableSpellSlots(1), 0);

			ok &= CheckCast("out of slots", DnDCasting.Cast(wizard, magicMissile, dummy), CastResult.NoSlotAvailable);

			// A long rest hands them all back.
			wizard.RestoreAllSpellSlots();
			ok &= CheckValue("slots after rest", wizard.GetAvailableSpellSlots(1), 2);

			// Class list and caster gating.
			ok &= CheckCast("wizard casting a cleric spell", DnDCasting.Cast(wizard, cureWounds, wizard), CastResult.NotOnClassList);
			ok &= CheckCast("fighter casting", DnDCasting.Cast(fighter, fireBolt, dummy), CastResult.NotACaster);
			ok &= CheckCast("fire bolt on self", DnDCasting.Cast(wizard, fireBolt, wizard), CastResult.WrongTargetType);

			ok &= CheckHealing();
			ok &= CheckSpellDamageIsApplied(wizard, fireBolt);

			Console.WriteLine(
				"[combat-selftest]   spellcasting: DC {0}, attack +{1}, {2} spell(s) available to a 1st-level wizard",
				Spellcasting.GetSaveDC(wizard),
				Spellcasting.GetSpellAttackBonus(wizard),
				SpellRegistry.GetAvailable(wizard).Count);

			wizard.Delete();
			dummy.Delete();

			return ok;
		}

		/// <summary>
		/// A spell's damage has to reach the target's hit points, not just be rolled. Fire Bolt is a
		/// spell attack, so it can miss - hence the retry loop.
		/// </summary>
		private static bool CheckSpellDamageIsApplied(DnDPlayerMobile caster, DnDSpell spell)
		{
			SrdGoblin victim = new SrdGoblin();
			victim.MoveToWorld(TestLocation, Map.Felucca);

			for (int i = 0; i < 200; i++)
			{
				victim.Hits = victim.HitsMax;

				int before = victim.Hits;

				if (DnDCasting.Cast(caster, spell, victim) == CastResult.Success && victim.Hits < before)
				{
					Console.WriteLine(
						"[combat-selftest]   spell damage: {0} took {1} from {2}",
						victim.Name,
						before - victim.Hits,
						spell.Name);

					victim.Delete();
					return true;
				}

				if (!victim.Alive)
				{
					Console.WriteLine("[combat-selftest]   spell damage: {0} was killed by {1}", victim.Name, spell.Name);

					victim.Delete();
					return true;
				}
			}

			Console.WriteLine("[combat-selftest] FAIL: 200 casts of {0} never damaged the target", spell.Name);

			victim.Delete();
			return false;
		}

		/// <summary>Healing has to actually put hit points back, and never past the maximum.</summary>
		private static bool CheckHealing()
		{
			DnDPlayerMobile cleric = new DnDPlayerMobile { Name = "SelfTestCleric", Body = 0x190 };

			cleric.ApplyDnDSetup(
				new AbilityScores(12, 10, 14, 10, 16, 10),
				CharacterClass.Parse("Cleric"));

			cleric.MoveToWorld(TestLocation, Map.Felucca);

			DnDSpell cureWounds = SpellRegistry.Find("Cure Wounds");

			cleric.Hits = 1;

			CastResult result = DnDCasting.Cast(cleric, cureWounds, cleric);

			bool ok = CheckCast("cure wounds", result, CastResult.Success);

			if (cleric.Hits <= 1)
			{
				Console.WriteLine("[combat-selftest] FAIL: cure wounds restored no hit points");
				ok = false;
			}

			Console.WriteLine("[combat-selftest]   healing: cleric HP 1 -> {0} (max {1})", cleric.Hits, cleric.HitsMax);

			if (cleric.Hits > cleric.HitsMax)
			{
				Console.WriteLine("[combat-selftest] FAIL: healing exceeded maximum hit points");
				ok = false;
			}

			cleric.Delete();

			return ok;
		}

		/// <summary>
		/// Experience awards, level thresholds, and that a level-up actually moves everything that
		/// keys off character level: hit points, proficiency bonus and spell slots.
		/// </summary>
		private static bool CheckAdvancement()
		{
			bool ok = true;

			// SRD experience by challenge rating, at the fractional boundaries and a whole rating.
			ok &= CheckValue("CR 1/8 XP", Advancement.GetExperienceForChallengeRating(0.125), 25);
			ok &= CheckValue("CR 1/4 XP", Advancement.GetExperienceForChallengeRating(0.25), 50);
			ok &= CheckValue("CR 1/2 XP", Advancement.GetExperienceForChallengeRating(0.5), 100);
			ok &= CheckValue("CR 1 XP", Advancement.GetExperienceForChallengeRating(1.0), 200);
			ok &= CheckValue("CR 5 XP", Advancement.GetExperienceForChallengeRating(5.0), 1800);

			// Level thresholds, including the boundary either side of level 2.
			ok &= CheckValue("level at 0 XP", Advancement.GetLevelForExperience(0), 1);
			ok &= CheckValue("level at 299 XP", Advancement.GetLevelForExperience(299), 1);
			ok &= CheckValue("level at 300 XP", Advancement.GetLevelForExperience(300), 2);
			ok &= CheckValue("level at 6500 XP", Advancement.GetLevelForExperience(6500), 5);
			ok &= CheckValue("level at 355000 XP", Advancement.GetLevelForExperience(355000), 20);

			// A goblin is CR 1/4, so the XML wiring has to produce 50 XP end to end.
			SrdGoblin goblin = new SrdGoblin();
			ok &= CheckValue("goblin XP value", goblin.ExperienceValue, 50);
			goblin.Delete();

			DnDPlayerMobile hero = new DnDPlayerMobile { Name = "SelfTestHero", Body = 0x190 };

			hero.ApplyDnDSetup(
				new AbilityScores(10, 10, 14, 16, 10, 10),
				CharacterClass.Parse("Wizard"));

			hero.MoveToWorld(TestLocation, Map.Felucca);

			// Wizard d6, Con +2: 6+2 at 1st, then 4+2 per level after.
			ok &= CheckValue("level 1 HP", hero.HitsMax, 8);
			ok &= CheckValue("level 1 slots", hero.GetMaxSpellSlots(1), 2);

			hero.AwardExperience(299);
			ok &= CheckValue("still level 1", hero.CharacterLevel, 1);

			hero.AwardExperience(1);
			ok &= CheckValue("level after 300 XP", hero.CharacterLevel, 2);
			ok &= CheckValue("level 2 HP", hero.HitsMax, 14);
			ok &= CheckValue("level 2 slots", hero.GetMaxSpellSlots(1), 3);

			// One award crossing several thresholds at once must apply every level it earns.
			hero.AwardExperience(6200);
			ok &= CheckValue("level after 6500 XP", hero.CharacterLevel, 5);
			ok &= CheckValue("level 5 proficiency", hero.CharacterClass.GetProficiencyBonus(hero.CharacterLevel), 3);
			ok &= CheckValue("level 5 3rd-level slots", hero.GetMaxSpellSlots(3), 2);
			ok &= CheckValue("level 5 cantrip dice", Spellcasting.GetCantripDice(hero.CharacterLevel), 2);

			// Levelling grants its hit points immediately rather than leaving the character hurt.
			ok &= CheckValue("HP kept up with level", hero.Hits, hero.HitsMax);

			// Rests.
			hero.ConsumeSpellSlot(1);
			hero.Hits = 1;
			hero.LongRest();

			ok &= CheckValue("HP after long rest", hero.Hits, hero.HitsMax);
			ok &= CheckValue("slots after long rest", hero.GetAvailableSpellSlots(1), hero.GetMaxSpellSlots(1));

			Console.WriteLine(
				"[combat-selftest]   advancement: level {0}, {1} XP, HP {2}, proficiency +{3}",
				hero.CharacterLevel,
				hero.Experience,
				hero.HitsMax,
				hero.CharacterClass.GetProficiencyBonus(hero.CharacterLevel));

			hero.Delete();

			return ok;
		}

		private static bool CheckCast(string label, CastResult actual, CastResult expected)
		{
			if (actual == expected)
			{
				return true;
			}

			Console.WriteLine("[combat-selftest] FAIL: {0} expected {1}, got {2}", label, expected, actual);
			return false;
		}

		private static bool CheckValue(string label, int actual, int expected)
		{
			if (actual == expected)
			{
				return true;
			}

			Console.WriteLine("[combat-selftest] FAIL: {0} expected {1}, got {2}", label, expected, actual);
			return false;
		}
	}
}
