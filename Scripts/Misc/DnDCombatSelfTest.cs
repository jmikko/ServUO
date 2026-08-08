using System;
using Server.Items;
using Server.Mobiles;

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

			ok &= CheckWeaponResolves(fighter);
			ok &= CheckWeaponResolves(goblin);

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
		/// Swings the resolver enough times to make the hit rate meaningful, then checks it against
		/// the rate the d20 math predicts. With a wide tolerance this catches a resolver that always
		/// hits, never hits, or ignores AC, without being flaky.
		/// </summary>
		/// <summary>
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
	}
}
