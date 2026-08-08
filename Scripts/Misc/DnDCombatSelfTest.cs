using System;
using System.Collections.Generic;
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

			ok &= CheckMovementWorks(fighter);
			ok &= CheckSpeciesRendering();
			ok &= CheckEquipmentTables();
			ok &= CheckSpellTable();
			ok &= CheckConditions();
			ok &= CheckConcentration();
			ok &= CheckAreaShapes();
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
			// unarmed 1d1 - must be what damage comes from. A longsword is Versatile, so with the
			// off-hand free it rolls its two-handed die.
			ok &= RunSwings("longsword, off-hand free", fighter, goblin, sword, 1, 10, 0, 3);

			// Fill the off-hand and the same weapon drops to its one-handed die.
			DnDShield shield = new DnDShield();
			fighter.EquipItem(shield);

			ok &= RunSwings("longsword + shield", fighter, goblin, sword, 1, 8, 0, 3);

			shield.Delete();

			// The creature's own attack: damage comes from its stat block, with no ability modifier
			// added on top (the stat block already bakes one in).
			ok &= RunSwings("goblin", goblin, fighter, null, 1, 6, 2, 0);

			ok &= CheckDamageIsApplied(fighter, goblin, sword);
			ok &= CheckSpellSlotTables();
			ok &= CheckSpellcasting(fighter);
			ok &= CheckAdvancement();
			ok &= CheckMulticlassing();
			ok &= CheckSkills();
			ok &= CheckAttunement();
			ok &= CheckSkillChoice();
			ok &= CheckLevelUpWireFormat();

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
		/// After a species change, no character may be left carrying hair or a beard that species
		/// has no art for. The client draws hair, beard and body as separate layers and does not
		/// check they belong together - it just draws them, which is what makes a human hairstyle
		/// on a gargoyle look like two overlapping figures rather than like an error.
		/// </summary>
		private static bool CheckSpeciesRendering()
		{
			bool ok = true;
			int bad = 0;

			foreach (Race race in Race.AllRaces)
			{
				if (race == null)
				{
					continue;
				}

				DnDPlayerMobile pm = new DnDPlayerMobile { Name = "RenderProbe", Body = 0x190 };

				// Human hair and beard, exactly as vanilla character creation hands them over.
				pm.HairItemID = 0x203B;
				pm.FacialHairItemID = 0x203E;

				pm.Race = race;

				bool hairOk = pm.HairItemID == 0 || race.ValidateHair(pm, pm.HairItemID);
				bool beardOk = pm.FacialHairItemID == 0 || race.ValidateFacialHair(pm, pm.FacialHairItemID);

				if (!hairOk || !beardOk)
				{
					Console.WriteLine(
						"[combat-selftest] FAIL: {0} kept art it has none for - hair 0x{1:X} beard 0x{2:X}",
						race.Name,
						pm.HairItemID,
						pm.FacialHairItemID);

					++bad;
					ok = false;
				}

				pm.Delete();
			}

			Console.WriteLine(
				"[combat-selftest]   species art: {0} race(s) checked, {1} mismatch(es)",
				Race.AllRaces.Count,
				bad);

			return ok;
		}

		/// <summary>
		/// Every spell row has to be coherent: on at least one class list, with dice if it deals
		/// damage or heals, and a save ability only where a save is actually rolled. A malformed
		/// row otherwise surfaces as a spell that silently does nothing when cast.
		/// </summary>
		private static bool CheckSpellTable()
		{
			bool ok = true;
			int cantrips = 0, levelled = 0, utility = 0;

			foreach (DnDSpell spell in SpellRegistry.AllSpells)
			{
				var data = spell as DataDrivenSpell;

				if (data == null)
				{
					continue; // hand-written spells carry their own logic
				}

				DnDSpellData row = data.Data;

				if (SpellRegistry.GetId(spell) < 0)
				{
					Console.WriteLine("[combat-selftest] FAIL: spell '{0}' has no id", row.Name);
					ok = false;
				}

				bool needsDice = row.Kind == SpellEffectKind.Damage || row.Kind == SpellEffectKind.Healing;

				if (needsDice && (row.DiceCount <= 0 || row.DiceSides <= 0))
				{
					Console.WriteLine("[combat-selftest] FAIL: {0} deals damage or healing with no dice", row.Name);
					ok = false;
				}

				if (row.Kind == SpellEffectKind.ArmorClass && row.ArmorClassValue <= 0)
				{
					Console.WriteLine("[combat-selftest] FAIL: {0} sets no armour class", row.Name);
					ok = false;
				}

				if (row.HalfOnSave && row.Resolution != SpellResolution.SavingThrow)
				{
					Console.WriteLine("[combat-selftest] FAIL: {0} halves on a save it never rolls", row.Name);
					ok = false;
				}

				if (row.Classes.Length == 0)
				{
					Console.WriteLine("[combat-selftest] FAIL: {0} is on no class list, so nobody can cast it", row.Name);
					ok = false;
				}

				if (row.Kind == SpellEffectKind.Utility)
				{
					++utility;
				}
				else if (row.Level == 0)
				{
					++cantrips;
				}
				else
				{
					++levelled;
				}
			}

			ok &= CheckMageArmor();
			ok &= CheckHighLevelSpell();
			ok &= CheckRollModifiers();

			Console.WriteLine(
				"[combat-selftest]   spells: {0} total - {1} damaging cantrip(s), {2} levelled, {3} utility placeholder(s)",
				SpellRegistry.Count,
				cantrips,
				levelled,
				utility);

			return ok;
		}

		/// <summary>
		/// Area shapes, checked by placing dummies at known offsets from a caster facing east.
		/// A cone is the one worth testing properly: it must widen with distance, must not reach
		/// behind the caster, and must not catch someone standing wide of its mouth.
		/// </summary>
		private static bool CheckAreaShapes()
		{
			DnDPlayerMobile caster = new DnDPlayerMobile { Name = "AreaProbe", Body = 0x190 };

			caster.ApplyDnDSetup(
				new AbilityScores(10, 10, 10, 16, 10, 10),
				CharacterClass.Parse("Wizard"));

			caster.MoveToWorld(TestLocation, Map.Felucca);

			// Dummies at offsets from the caster; the cone will be aimed due east.
			var placed = new List<Mobile>();

			Func<int, int, Mobile> place = (dx, dy) =>
			{
				var m = new SrdGoblin { Blessed = true };

				m.MoveToWorld(new Point3D(TestLocation.X + dx, TestLocation.Y + dy, TestLocation.Z), Map.Felucca);
				placed.Add(m);

				return m;
			};

			Mobile aim = place(3, 0);      // straight ahead, at the far edge
			Mobile near = place(1, 0);     // straight ahead, close
			Mobile wide = place(1, 2);     // beside the mouth - too wide to be caught at distance 1
			Mobile spread = place(3, 1);   // far along, where the cone is wide enough
			Mobile behind = place(-2, 0);  // behind the caster
			Mobile beyond = place(6, 0);   // straight ahead but past the end

			var cone = DnDSpellArea.GetTargets(caster, aim, SpellShape.Cone, 3, false);

			bool ok = true;

			ok &= CheckContains("cone catches the aim point", cone, aim, true);
			ok &= CheckContains("cone catches close ahead", cone, near, true);
			ok &= CheckContains("cone widens with distance", cone, spread, true);
			ok &= CheckContains("cone spares someone wide of its mouth", cone, wide, false);
			ok &= CheckContains("cone does not reach behind", cone, behind, false);
			ok &= CheckContains("cone stops at its length", cone, beyond, false);
			ok &= CheckContains("cone spares its caster", cone, caster, false);

			// A line is one tile wide however far it runs.
			var line = DnDSpellArea.GetTargets(caster, aim, SpellShape.Line, 6, false);

			ok &= CheckContains("line catches straight ahead", line, beyond, true);
			ok &= CheckContains("line spares those to the side", line, spread, false);

			// A sphere is centred on the target, not the caster, and does not care about facing.
			var sphere = DnDSpellArea.GetTargets(caster, aim, SpellShape.Sphere, 2, false);

			ok &= CheckContains("sphere catches near its centre", sphere, spread, true);
			ok &= CheckContains("sphere ignores distant targets", sphere, beyond, false);

			Console.WriteLine(
				"[combat-selftest]   area shapes: cone caught {0}, line {1}, sphere {2}",
				cone.Count,
				line.Count,
				sphere.Count);

			foreach (Mobile m in placed)
			{
				m.Delete();
			}

			caster.Delete();

			return ok;
		}

		private static bool CheckContains(string label, List<Mobile> targets, Mobile m, bool expected)
		{
			bool actual = targets.Contains(m);

			if (actual == expected)
			{
				return true;
			}

			Console.WriteLine(
				"[combat-selftest] FAIL: {0} - expected {1}, got {2}", label, expected, actual);

			return false;
		}

		/// <summary>
		/// Conditions have to actually swing the dice. A blinded attacker should hit noticeably
		/// less often and a paralysed defender noticeably more, and one of each should cancel back
		/// to roughly the normal rate - that cancelling is the part people get wrong.
		/// </summary>
		private static bool CheckConditions()
		{
			const int Rolls = 4000;

			DnDPlayerMobile attacker = new DnDPlayerMobile { Name = "ConditionProbe", Body = 0x190 };

			attacker.ApplyDnDSetup(
				new AbilityScores(16, 12, 14, 10, 10, 10),
				CharacterClass.Parse("Fighter"));

			attacker.MoveToWorld(TestLocation, Map.Felucca);

			SrdGoblin defender = new SrdGoblin { Blessed = true };
			defender.MoveToWorld(TestLocation, Map.Felucca);

			double normal = MeasureHitRate(attacker, defender, Rolls);

			DnDConditions.Add(attacker, DnDCondition.Blinded, TimeSpan.FromMinutes(5));
			double blinded = MeasureHitRate(attacker, defender, Rolls);

			DnDConditions.Add(defender, DnDCondition.Paralyzed, TimeSpan.FromMinutes(5));
			double cancelled = MeasureHitRate(attacker, defender, Rolls);

			DnDConditions.Clear(attacker);
			double advantaged = MeasureHitRate(attacker, defender, Rolls);

			Console.WriteLine(
				"[combat-selftest]   conditions: normal {0:P1}, blinded {1:P1}, blinded+paralysed {2:P1}, paralysed {3:P1}",
				normal,
				blinded,
				cancelled,
				advantaged);

			bool ok = true;

			if (blinded >= normal)
			{
				Console.WriteLine("[combat-selftest] FAIL: disadvantage did not lower the hit rate");
				ok = false;
			}

			if (advantaged <= normal)
			{
				Console.WriteLine("[combat-selftest] FAIL: advantage did not raise the hit rate");
				ok = false;
			}

			// One of each cancels, so this should sit near the unmodified rate rather than at
			// either extreme.
			if (Math.Abs(cancelled - normal) > 0.05)
			{
				Console.WriteLine("[combat-selftest] FAIL: advantage and disadvantage did not cancel out");
				ok = false;
			}

			// A paralysed creature simply fails Dexterity saves.
			if (CombatRules.CheckSave(defender, AbilityScoreType.Dex, 5))
			{
				Console.WriteLine("[combat-selftest] FAIL: a paralysed creature passed a Dexterity save");
				ok = false;
			}

			DnDConditions.Clear(defender);
			attacker.Delete();
			defender.Delete();

			return ok;
		}

		private static double MeasureHitRate(Mobile attacker, Mobile defender, int rolls)
		{
			int hits = 0;

			for (int i = 0; i < rolls; i++)
			{
				if (DnDCombat.RollAttack(attacker, defender, null).Hit)
				{
					++hits;
				}
			}

			return hits / (double)rolls;
		}

		/// <summary>
		/// Concentration holds one spell at a time and a second displaces the first. Damage risks
		/// breaking it, so enough damage should eventually break it.
		/// </summary>
		private static bool CheckConcentration()
		{
			DnDPlayerMobile caster = new DnDPlayerMobile { Name = "ConcentrationProbe", Body = 0x190 };

			caster.ApplyDnDSetup(
				new AbilityScores(10, 10, 10, 16, 10, 10),
				CharacterClass.Parse("Wizard"));

			caster.MoveToWorld(TestLocation, Map.Felucca);

			bool ok = true;

			DnDConcentration.Begin(caster, "First Spell", TimeSpan.FromMinutes(10), null);
			ok &= CheckText("concentrating", DnDConcentration.GetSpellName(caster), "First Spell");

			// Starting another drops the first - a caster only ever holds one.
			DnDConcentration.Begin(caster, "Second Spell", TimeSpan.FromMinutes(10), null);
			ok &= CheckText("second spell displaces first", DnDConcentration.GetSpellName(caster), "Second Spell");

			// A big enough hit should break it within a handful of attempts; the save is DC 10 or
			// half the damage, so 60 damage is DC 30 and essentially unmakeable.
			DnDConcentration.OnDamaged(caster, 60);

			if (DnDConcentration.IsConcentrating(caster))
			{
				Console.WriteLine("[combat-selftest] FAIL: concentration survived a DC 30 save");
				ok = false;
			}

			Console.WriteLine("[combat-selftest]   concentration: one spell at a time, broken by heavy damage");

			caster.Delete();

			return ok;
		}

		/// <summary>
		/// Earns experience and then spends every level it granted on one class.
		/// <para>
		/// Levelling is two steps now: experience grants PENDING levels, and the player chooses
		/// which class each one goes into - that choice is what multiclassing is. Tests that only
		/// award experience leave a character sitting at their old level with an unanswered prompt,
		/// which is exactly what a player who ignores the gump gets.
		/// </para>
		/// </summary>
		private static void AwardAndLevel(DnDPlayerMobile pm, int experience, CharacterClass into)
		{
			pm.AwardExperience(experience);

			// Guard rather than while(PendingLevels > 0): if AddClassLevel ever stopped consuming
			// one, the loop would hang the server at boot rather than failing a check.
			for (int i = 0; i < Advancement.MaxLevel && pm.PendingLevels > 0; ++i)
			{
				pm.AddClassLevel(into);
			}
		}

		/// <summary>
		/// The level-up submission's wire format, checked by feeding the server's reader exactly
		/// the bytes the client writes.
		/// <para>
		/// This is the one shape of bug the rest of the self-test cannot see: both sides compiled,
		/// both were self-consistent, and they simply disagreed about the format. The client wrote a
		/// bare ASCII name; the server expected a type tag, a length and UTF-16. The tag check
		/// failed, the class name came back empty, the level was silently not applied - and every
		/// field after it was misread too, because the failed read still consumed a byte.
		/// </para>
		/// </summary>
		private static bool CheckLevelUpWireFormat()
		{
			const string ClassName = "Fighter";

			var bytes = new List<byte>();

			// A variable-length packet's id and length, which PacketReader skips over - it starts
			// reading at byte 3. Leaving these out makes the test read from the middle of its own
			// payload and fail for a reason that has nothing to do with the format under test.
			bytes.Add(0xD7);
			bytes.Add(0);
			bytes.Add(0);

			// Exactly what OutgoingPackets.WriteEncodedString emits: tag 2, character count as a
			// big-endian ushort, then big-endian UTF-16.
			bytes.Add(2);
			bytes.Add((byte)(ClassName.Length >> 8));
			bytes.Add((byte)ClassName.Length);

			foreach (char c in ClassName)
			{
				bytes.Add((byte)(c >> 8));
				bytes.Add((byte)c);
			}

			// Then the six ability increases, as type-tagged int32s.
			for (int i = 0; i < 6; ++i)
			{
				bytes.Add(0);
				bytes.Add(0);
				bytes.Add(0);
				bytes.Add(0);
				bytes.Add((byte)(i == 0 ? 1 : 0));
			}

			byte[] data = bytes.ToArray();

			var reader = new Network.EncodedReader(new Network.PacketReader(data, data.Length, false));

			string parsed = reader.ReadUnicodeStringSafe();

			bool ok = CheckText("level-up class name survives the wire", parsed, ClassName);

			// The misalignment is the part that makes this hard to spot by eye: if the string read
			// is wrong, the numbers after it are wrong too, and nothing reports an error.
			int firstIncrease = reader.ReadInt32();

			ok &= CheckValue("field after the name still aligned", firstIncrease, 1);

			if (CharacterClass.Parse(parsed) == null)
			{
				Console.WriteLine("[combat-selftest] FAIL: '{0}' does not resolve to a class", parsed);
				ok = false;
			}

			Console.WriteLine("[combat-selftest]   level-up wire format: '{0}' parsed, next field {1}", parsed, firstIncrease);

			return ok;
		}

		/// <summary>
		/// Skill proficiency choice. The client is not trusted, so the class has to honour legal
		/// picks, discard illegal ones, and top up anything the player left unfilled - a character
		/// must never end up with fewer proficiencies than the rules grant just because the UI
		/// failed to ask.
		/// </summary>
		private static bool CheckSkillChoice()
		{
			bool ok = true;

			CharacterClass wizard = CharacterClass.Parse("Wizard");
			CharacterClass rogue = CharacterClass.Parse("Rogue");

			// Every class must offer at least as many skills as it lets a character take.
			foreach (CharacterClass c in CharacterClass.AllClasses)
			{
				if (c.SkillChoices.Length < c.SkillChoiceCount)
				{
					Console.WriteLine(
						"[combat-selftest] FAIL: {0} offers {1} skill(s) but allows {2}",
						c.Name,
						c.SkillChoices.Length,
						c.SkillChoiceCount);

					ok = false;
				}
			}

			DnDPlayerMobile pm = new DnDPlayerMobile { Name = "SkillChoiceProbe", Body = 0x190 };

			pm.ApplyDnDSetup(new AbilityScores(10, 10, 10, 10, 10, 10), wizard);
			pm.MoveToWorld(TestLocation, Map.Felucca);

			// A legal pick is honoured exactly.
			pm.ApplySkillProficiencies(wizard, new[] { DnDSkill.Investigation, DnDSkill.Religion });

			ok &= CheckValue("legal pick honoured", pm.IsProficient(DnDSkill.Investigation) ? 1 : 0, 1);
			ok &= CheckValue("second legal pick honoured", pm.IsProficient(DnDSkill.Religion) ? 1 : 0, 1);

			// Stealth is not on the Wizard list, so it must be refused - and the slot it would have
			// taken filled from the class' own skills rather than left empty.
			pm.ApplySkillProficiencies(wizard, new[] { DnDSkill.Stealth, DnDSkill.Arcana });

			ok &= CheckValue("skill off the class list refused", pm.IsProficient(DnDSkill.Stealth) ? 1 : 0, 0);
			ok &= CheckValue("legal pick still taken", pm.IsProficient(DnDSkill.Arcana) ? 1 : 0, 1);
			ok &= CheckValue("refused slot topped up", CountProficiencies(pm), wizard.SkillChoiceCount);

			// The same skill twice must not consume two slots.
			pm.ApplySkillProficiencies(wizard, new[] { DnDSkill.Arcana, DnDSkill.Arcana });

			ok &= CheckValue("duplicate does not fill two slots", CountProficiencies(pm), wizard.SkillChoiceCount);

			// No choice at all still produces a full, legal set - the old-client path.
			pm.ApplySkillProficiencies(rogue, null);

			ok &= CheckValue("defaults fill when nothing chosen", CountProficiencies(pm), rogue.SkillChoiceCount);

			// More picks than allowed must be truncated, not obeyed.
			pm.ApplySkillProficiencies(wizard, wizard.SkillChoices);

			ok &= CheckValue("over-long choice truncated", CountProficiencies(pm), wizard.SkillChoiceCount);

			Console.WriteLine(
				"[combat-selftest]   skill choice: {0} class(es) checked, Rogue picks {1}, Wizard {2}",
				CharacterClass.AllClasses.Count,
				rogue.SkillChoiceCount,
				wizard.SkillChoiceCount);

			pm.Delete();

			return ok;
		}

		private static int CountProficiencies(DnDPlayerMobile pm)
		{
			int count = 0;

			foreach (DnDSkill skill in Enum.GetValues(typeof(DnDSkill)))
			{
				if (pm.IsProficient(skill))
				{
					++count;
				}
			}

			return count;
		}

		/// <summary>
		/// Multiclassing splits one number into two that are easy to confuse. Proficiency bonus must
		/// come from TOTAL level - otherwise multiclassing is a way to farm it - while saving throw
		/// proficiencies must come from the STARTING class only.
		/// </summary>
		private static bool CheckMulticlassing()
		{
			DnDPlayerMobile pm = new DnDPlayerMobile { Name = "MulticlassProbe", Body = 0x190 };

			pm.ApplyDnDSetup(
				new AbilityScores(14, 14, 14, 14, 14, 14),
				CharacterClass.Parse("Fighter"));

			pm.MoveToWorld(TestLocation, Map.Felucca);

			bool ok = CheckValue("starts level 1", pm.TotalLevel, 1);

			// Five Fighter levels, then three Wizard: 8 total, so proficiency is +3.
			AwardAndLevel(pm, Advancement.GetExperienceForLevel(5), CharacterClass.Parse("Fighter"));
			AwardAndLevel(pm, Advancement.GetExperienceForLevel(8) - pm.Experience, CharacterClass.Parse("Wizard"));

			ok &= CheckValue("total level", pm.TotalLevel, 8);
			ok &= CheckValue("classes held", pm.Classes.Count, 2);

			ok &= CheckValue(
				"proficiency from total level",
				pm.PrimaryClass.GetProficiencyBonus(pm.TotalLevel),
				3);

			// The starting class stays the starting class, however many levels go elsewhere.
			ok &= CheckText("primary class unchanged", pm.PrimaryClass.Name, "Fighter");

			// Saving throws follow the Fighter (Str, Con) and NOT the Wizard (Int, Wis), which is
			// the rule multiclassing most often gets wrong.
			if (!pm.PrimaryClass.IsProficientSave(AbilityScoreType.Str))
			{
				Console.WriteLine("[combat-selftest] FAIL: lost the starting class' save proficiency");
				ok = false;
			}

			if (pm.PrimaryClass.IsProficientSave(AbilityScoreType.Int))
			{
				Console.WriteLine("[combat-selftest] FAIL: gained a save proficiency from a later class");
				ok = false;
			}

			// Slots come only from the caster levels - three Wizard levels here, not the total of
			// eight. A 3rd-level full caster has 4 first-level slots and 2 second-level.
			ok &= CheckValue("wizard levels", pm.Classes[CharacterClass.Parse("Wizard")], 3);
			ok &= CheckValue("multiclass 1st-level slots", pm.GetMaxSpellSlots(1), 4);
			ok &= CheckValue("multiclass 2nd-level slots", pm.GetMaxSpellSlots(2), 2);
			ok &= CheckValue("no slots from fighter levels", pm.GetMaxSpellSlots(4), 0);

			Console.WriteLine(
				"[combat-selftest]   multiclass: {0} at total level {1}, proficiency +{2}, slots {3}/{4}",
				DescribeClasses(pm),
				pm.TotalLevel,
				pm.PrimaryClass.GetProficiencyBonus(pm.TotalLevel),
				pm.GetMaxSpellSlots(1),
				pm.GetMaxSpellSlots(2));

			pm.Delete();

			return ok;
		}

		private static string DescribeClasses(DnDPlayerMobile pm)
		{
			var parts = new List<string>();

			foreach (var entry in pm.Classes)
			{
				parts.Add(String.Format("{0} {1}", entry.Key.Name, entry.Value));
			}

			return String.Join(" / ", parts);
		}

		/// <summary>
		/// Skill checks must add the proficiency bonus only where the character is proficient, and
		/// must read the right ability for the skill. Measured statistically, because a check that
		/// ignores proficiency entirely still passes a "did it return true sometimes" test.
		/// </summary>
		private static bool CheckSkills()
		{
			const int Rolls = 6000;

			DnDPlayerMobile rogue = new DnDPlayerMobile { Name = "SkillProbe", Body = 0x190 };

			rogue.ApplyDnDSetup(
				new AbilityScores(10, 10, 10, 10, 10, 10),
				CharacterClass.Parse("Rogue"));

			rogue.MoveToWorld(TestLocation, Map.Felucca);

			bool ok = true;

			// Every skill has to name an ability, or a check against it silently uses the wrong one.
			foreach (DnDSkill skill in Enum.GetValues(typeof(DnDSkill)))
			{
				AbilityScoreType ability = DnDSkills.GetPrimaryAbility(skill);

				if (!Enum.IsDefined(typeof(AbilityScoreType), ability))
				{
					Console.WriteLine("[combat-selftest] FAIL: skill {0} maps to no ability", skill);
					ok = false;
				}
			}

			// With every ability at 10 the modifier is 0, so any difference between a proficient and
			// a non-proficient skill is the proficiency bonus and nothing else.
			DnDSkill proficient = DnDSkill.Stealth, unproficient = DnDSkill.Stealth;
			bool foundPair = false;

			foreach (DnDSkill skill in Enum.GetValues(typeof(DnDSkill)))
			{
				if (rogue.IsProficient(skill))
				{
					proficient = skill;
				}
				else if (DnDSkills.GetPrimaryAbility(skill) == DnDSkills.GetPrimaryAbility(proficient))
				{
					unproficient = skill;
					foundPair = true;
				}
			}

			if (!foundPair)
			{
				Console.WriteLine("[combat-selftest]   skills: no comparable pair to measure, proficiency unverified");
			}
			else
			{
				double withProficiency = MeasureSkillRate(rogue, proficient, 13, Rolls);
				double without = MeasureSkillRate(rogue, unproficient, 13, Rolls);

				Console.WriteLine(
					"[combat-selftest]   skills: {0} (proficient) {1:P1} vs {2} {3:P1} against DC 13",
					proficient,
					withProficiency,
					unproficient,
					without);

				if (withProficiency <= without)
				{
					Console.WriteLine("[combat-selftest] FAIL: proficiency did not improve the skill check");
					ok = false;
				}
			}

			rogue.Delete();

			return ok;
		}

		private static double MeasureSkillRate(Mobile m, DnDSkill skill, int dc, int rolls)
		{
			int passes = 0;

			for (int i = 0; i < rolls; i++)
			{
				if (CombatRules.CheckSkill(m, skill, dc))
				{
					++passes;
				}
			}

			return passes / (double)rolls;
		}

		/// <summary>
		/// Attunement, and the thing it is for: a magic item's ability score override has to reach
		/// the rules, not just the character sheet. EffectiveAbilityScores exists precisely so that
		/// an Amulet of Health raises saving throws as well as hit points - reading the raw scores
		/// anywhere in the rules would silently half-apply every such item.
		/// </summary>
		private static bool CheckAttunement()
		{
			DnDPlayerMobile pm = new DnDPlayerMobile { Name = "AttuneProbe", Body = 0x190 };

			pm.ApplyDnDSetup(
				new AbilityScores(10, 10, 10, 10, 10, 10),
				CharacterClass.Parse("Fighter"));

			pm.MoveToWorld(TestLocation, Map.Felucca);

			var amulet = new DnDAmuletOfHealth();

			pm.AddToBackpack(amulet);

			bool ok = CheckValue("not attuned yet", pm.EffectiveAbilityScores.Con, 10);

			pm.AttunedItems.Add(amulet);

			ok &= CheckValue("attuned", pm.IsAttunedTo(amulet) ? 1 : 0, 1);

			int effectiveCon = pm.EffectiveAbilityScores.Con;

			if (effectiveCon <= 10)
			{
				Console.WriteLine("[combat-selftest] FAIL: attuning the amulet did not raise Constitution");
				ok = false;
			}

			// The raw score must NOT move - the override is an effective value, not a stat edit.
			ok &= CheckValue("raw score untouched", pm.AbilityScores.Con, 10);

			Console.WriteLine(
				"[combat-selftest]   attunement: Con {0} raw, {1} effective, {2} item(s) attuned",
				pm.AbilityScores.Con,
				effectiveCon,
				pm.AttunedItems.Count);

			pm.AttunedItems.Clear();
			amulet.Delete();
			pm.Delete();

			return ok;
		}

		private static bool CheckText(string label, string actual, string expected)
		{
			if (actual == expected)
			{
				return true;
			}

			Console.WriteLine("[combat-selftest] FAIL: {0} expected '{1}', got '{2}'", label, expected, actual);
			return false;
		}

		/// <summary>
		/// The Bless and Bane family. A d4 averages 2.5, which against a d20 is worth about 12.5
		/// percentage points of hit rate, so the shift is large enough to measure but small enough
		/// that measuring it proves the die is actually being rolled rather than a flat bonus.
		/// </summary>
		private static bool CheckRollModifiers()
		{
			const int Rolls = 6000;

			DnDPlayerMobile fighter = new DnDPlayerMobile { Name = "ModifierProbe", Body = 0x190 };

			fighter.ApplyDnDSetup(
				new AbilityScores(16, 12, 14, 10, 10, 10),
				CharacterClass.Parse("Fighter"));

			fighter.MoveToWorld(TestLocation, Map.Felucca);

			SrdGoblin dummy = new SrdGoblin { Blessed = true };
			dummy.MoveToWorld(TestLocation, Map.Felucca);

			double plain = MeasureHitRate(fighter, dummy, Rolls);

			DnDRollModifiers.Add(fighter, "Bless", 4, 1, RollKind.Attack, TimeSpan.FromMinutes(5), false);
			double blessed = MeasureHitRate(fighter, dummy, Rolls);

			DnDRollModifiers.Remove(fighter, "Bless");
			DnDRollModifiers.Add(fighter, "Bane", 4, -1, RollKind.Attack, TimeSpan.FromMinutes(5), false);
			double baned = MeasureHitRate(fighter, dummy, Rolls);

			DnDRollModifiers.Clear(fighter);

			Console.WriteLine(
				"[combat-selftest]   roll modifiers: plain {0:P1}, blessed {1:P1}, baned {2:P1}",
				plain,
				blessed,
				baned);

			bool ok = true;

			// A d4 is worth 2.5 on a d20, so roughly 12.5 points either way. Allow a wide band.
			if (blessed - plain < 0.05 || blessed - plain > 0.20)
			{
				Console.WriteLine("[combat-selftest] FAIL: Bless did not shift the hit rate by about a d4");
				ok = false;
			}

			if (plain - baned < 0.05 || plain - baned > 0.20)
			{
				Console.WriteLine("[combat-selftest] FAIL: Bane did not shift the hit rate by about a d4");
				ok = false;
			}

			// One-shot modifiers are spent by the first roll that consults them.
			DnDRollModifiers.Add(fighter, "Resistance", 4, 1, RollKind.Save, TimeSpan.FromMinutes(5), true);

			if (!DnDRollModifiers.Has(fighter, RollKind.Save))
			{
				Console.WriteLine("[combat-selftest] FAIL: Resistance did not attach");
				ok = false;
			}

			DnDRollModifiers.Roll(fighter, RollKind.Save);

			if (DnDRollModifiers.Has(fighter, RollKind.Save))
			{
				Console.WriteLine("[combat-selftest] FAIL: a one-shot modifier survived being used");
				ok = false;
			}

			// Recasting replaces rather than stacking.
			DnDRollModifiers.Add(fighter, "Bless", 4, 1, RollKind.Attack, TimeSpan.FromMinutes(5), false);
			DnDRollModifiers.Add(fighter, "Bless", 4, 1, RollKind.Attack, TimeSpan.FromMinutes(5), false);

			double doubled = MeasureHitRate(fighter, dummy, Rolls);

			if (doubled - blessed > 0.08)
			{
				Console.WriteLine("[combat-selftest] FAIL: Bless stacked with itself");
				ok = false;
			}

			DnDRollModifiers.Clear(fighter);
			fighter.Delete();
			dummy.Delete();

			return ok;
		}

		/// <summary>
		/// Fireball, end to end: a 5th-level wizard must be offered it, be refused it at 1st, and
		/// have it actually burn a crowd when cast. This is the check that the levelled half of the
		/// catalogue is reachable at all, rather than merely well-formed.
		/// </summary>
		private static bool CheckHighLevelSpell()
		{
			DnDSpell fireball = SpellRegistry.Find("Fireball");

			if (fireball == null)
			{
				Console.WriteLine("[combat-selftest] FAIL: Fireball is not registered");
				return false;
			}

			DnDPlayerMobile novice = new DnDPlayerMobile { Name = "NoviceProbe", Body = 0x190 };

			novice.ApplyDnDSetup(
				new AbilityScores(10, 12, 12, 16, 10, 10),
				CharacterClass.Parse("Wizard"));

			novice.MoveToWorld(TestLocation, Map.Felucca);

			novice.KnownSpells.Add(SpellRegistry.GetId(fireball));

			// A 1st-level wizard has no 3rd-level slot, so the spell is on the list but unreachable.
			bool ok = !SpellRegistry.GetAvailable(novice).Contains(fireball);

			if (!ok)
			{
				Console.WriteLine("[combat-selftest] FAIL: a 1st-level wizard was offered Fireball");
			}

			// Level up to 5th, which is where the third-level slots arrive.
			AwardAndLevel(novice, 6500, novice.PrimaryClass);

			ok &= CheckValue("wizard level for Fireball", novice.TotalLevel, 5);

			if (!SpellRegistry.GetAvailable(novice).Contains(fireball))
			{
				Console.WriteLine("[combat-selftest] FAIL: a 5th-level wizard was not offered Fireball");
				Console.WriteLine("[combat-selftest] DEBUG: highest slot = {0}, known spells count = {1}, knows fireball = {2}", 
					Spellcasting.GetHighestSlotLevel(novice.PrimaryClass.SpellProgression, novice.TotalLevel),
					novice.KnownSpells.Count,
					novice.KnownSpells.Contains(SpellRegistry.GetId(fireball)));
				foreach(var s in SpellRegistry.GetAvailable(novice)) {
					Console.WriteLine("[combat-selftest] DEBUG available: " + s.Name);
				}
				ok = false;
			}

			// Three victims clustered together, so the sphere has something to catch.
			var victims = new List<SrdGoblin>();

			for (int i = 0; i < 3; i++)
			{
				var goblin = new SrdGoblin();

				goblin.MoveToWorld(
					new Point3D(TestLocation.X + 6 + i, TestLocation.Y, TestLocation.Z), Map.Felucca);

				victims.Add(goblin);
			}

			int before = 0;

			foreach (SrdGoblin goblin in victims)
			{
				before += goblin.Hits;
			}

			ok &= CheckCast("fireball", DnDCasting.Cast(novice, fireball, victims[1]), CastResult.Success);

			int after = 0;
			int killed = 0;

			foreach (SrdGoblin goblin in victims)
			{
				after += goblin.Hits;

				if (!goblin.Alive || goblin.Deleted)
				{
					++killed;
				}
			}

			Console.WriteLine(
				"[combat-selftest]   fireball: {0} goblin(s) took {1} total damage, {2} killed",
				victims.Count,
				before - after,
				killed);

			if (before - after <= 0 && killed == 0)
			{
				Console.WriteLine("[combat-selftest] FAIL: Fireball harmed nobody");
				ok = false;
			}

			ok &= CheckValue("3rd-level slots spent", novice.GetAvailableSpellSlots(3), 1);

			foreach (SrdGoblin goblin in victims)
			{
				goblin.Delete();
			}

			novice.Delete();

			return ok;
		}

		/// <summary>
		/// Mage Armor sets a floor rather than adding, so it has to help an unarmoured wizard and
		/// do nothing at all for someone already better protected.
		/// </summary>
		private static bool CheckMageArmor()
		{
			DnDPlayerMobile wizard = new DnDPlayerMobile { Name = "MageArmorProbe", Body = 0x190 };

			wizard.ApplyDnDSetup(
				new AbilityScores(10, 14, 12, 16, 10, 10),
				CharacterClass.Parse("Wizard"));

			wizard.MoveToWorld(TestLocation, Map.Felucca);

			bool ok = CheckValue("AC before Mage Armor", wizard.ArmorClass, 12); // 10 + 2

			DnDSpell mageArmor = SpellRegistry.Find("Mage Armor");

			if (mageArmor == null)
			{
				Console.WriteLine("[combat-selftest] FAIL: Mage Armor is not registered");
				wizard.Delete();
				return false;
			}

			ok &= CheckCast("mage armor", DnDCasting.Cast(wizard, mageArmor, wizard), CastResult.Success);
			ok &= CheckValue("AC after Mage Armor", wizard.ArmorClass, 15); // 13 + 2

			Spells.DnD.DnDEffects.Clear(wizard);
			wizard.Delete();

			return ok;
		}

		/// <summary>
		/// Every weapon and armour row has to instantiate, and every generated class has to have a
		/// row behind it. A typo in either direction throws only when someone spawns that one item,
		/// which on a 50-item table is a bad way to find out.
		/// </summary>
		private static bool CheckEquipmentTables()
		{
			bool ok = true;
			int weapons = 0, armor = 0;

			foreach (DnDWeaponData data in DnDEquipmentTable.Weapons)
			{
				Type type = ScriptCompiler.FindTypeByName("DnD" + data.Id);

				if (type == null)
				{
					Console.WriteLine("[combat-selftest] FAIL: weapon '{0}' has no DnD{0} class", data.Id);
					ok = false;
					continue;
				}

				DnDWeapon weapon = Activator.CreateInstance(type) as DnDWeapon;

				if (weapon == null)
				{
					Console.WriteLine("[combat-selftest] FAIL: DnD{0} is not a DnDWeapon", data.Id);
					ok = false;
					continue;
				}

				int min, max;
				CombatRules.GetDiceRange(weapon.DamageDiceExpression, out min, out max);

				if (min <= 0 || max < min)
				{
					Console.WriteLine(
						"[combat-selftest] FAIL: {0} has an unusable damage expression '{1}'",
						data.Id,
						weapon.DamageDiceExpression);

					ok = false;
				}

				// A two-handed weapon that is also versatile makes no sense - versatile exists
				// precisely to describe what happens when the other hand is free.
				if (data.Has(WeaponProperty.TwoHanded) && data.Has(WeaponProperty.Versatile))
				{
					Console.WriteLine("[combat-selftest] FAIL: {0} is both TwoHanded and Versatile", data.Id);
					ok = false;
				}

				if (data.Has(WeaponProperty.Versatile) && String.IsNullOrEmpty(data.VersatileDamage))
				{
					Console.WriteLine("[combat-selftest] FAIL: {0} is Versatile with no two-handed die", data.Id);
					ok = false;
				}

				++weapons;
				weapon.Delete();
			}

			foreach (DnDArmorData data in DnDEquipmentTable.Armor)
			{
				Type type = ScriptCompiler.FindTypeByName("DnD" + data.Id);

				if (type == null)
				{
					Console.WriteLine("[combat-selftest] FAIL: armour '{0}' has no DnD{0} class", data.Id);
					ok = false;
					continue;
				}

				DnDArmor piece = Activator.CreateInstance(type) as DnDArmor;

				if (piece == null)
				{
					Console.WriteLine("[combat-selftest] FAIL: DnD{0} is not a DnDArmor", data.Id);
					ok = false;
					continue;
				}

				if (piece.ArmorBonus <= 0)
				{
					Console.WriteLine("[combat-selftest] FAIL: {0} grants no armour class", data.Id);
					ok = false;
				}

				++armor;
				piece.Delete();
			}

			ok &= CheckArmorClassMath();

			Console.WriteLine(
				"[combat-selftest]   equipment: {0} weapon(s), {1} armour piece(s) all instantiate",
				weapons,
				armor);

			return ok;
		}

		/// <summary>
		/// The three Dexterity-cap behaviours, plus a shield stacking on top, against a character
		/// whose Dexterity is high enough for the cap to actually bite.
		/// </summary>
		private static bool CheckArmorClassMath()
		{
			DnDPlayerMobile pm = new DnDPlayerMobile { Name = "ArmorProbe", Body = 0x190 };

			// Str 15 so heavy armour is wearable; Dex 18 (+4) so the caps are visible.
			pm.ApplyDnDSetup(
				new AbilityScores(15, 18, 12, 10, 10, 10),
				CharacterClass.Parse("Fighter"));

			bool ok = true;

			ok &= CheckValue("AC unarmoured", pm.ArmorClass, 14); // 10 + 4

			ok &= CheckArmorPiece(pm, new DnDLeatherArmor(), "AC leather", 15);   // 11 + 4 uncapped
			ok &= CheckArmorPiece(pm, new DnDChainShirt(), "AC chain shirt", 15); // 13 + 2 capped
			ok &= CheckArmorPiece(pm, new DnDPlateArmor(), "AC plate", 18);       // 18 + 0 capped

			// Shield stacks on top of whatever body armour is worn.
			DnDPlateArmor plate = new DnDPlateArmor();
			DnDShield shield = new DnDShield();

			pm.EquipItem(plate);
			pm.EquipItem(shield);

			ok &= CheckValue("AC plate + shield", pm.ArmorClass, 20);

			plate.Delete();
			shield.Delete();
			pm.Delete();

			return ok;
		}

		private static bool CheckArmorPiece(DnDPlayerMobile pm, DnDArmor piece, string label, int expected)
		{
			pm.EquipItem(piece);

			bool ok = CheckValue(label, pm.ArmorClass, expected);

			piece.Delete();

			return ok;
		}

		/// <summary>
		/// Nothing can move unless something has assigned Movement.Impl - it is null by default and
		/// CheckMovement then refuses every step, silently, for players and creatures alike. That is
		/// exactly what happened once the pathing service was parked, and it is invisible from the
		/// server console, so it gets a check of its own.
		/// </summary>
		private static bool CheckMovementWorks(Mobile m)
		{
			if (Movement.Movement.Impl == null)
			{
				Console.WriteLine("[combat-selftest] FAIL: Movement.Impl is unset - nothing will be able to walk");
				return false;
			}

			// A step off the test tile has to be permitted by the real implementation.
			int walkable = 0;

			for (int i = 0; i < 8; i++)
			{
				int newZ;

				if (Movement.Movement.CheckMovement(m, m.Map, m.Location, (Direction)i, out newZ))
				{
					++walkable;
				}
			}

			Console.WriteLine(
				"[combat-selftest]   movement: {0} ({1}/8 directions walkable from the test tile)",
				Movement.Movement.Impl.GetType().Name,
				walkable);

			if (walkable == 0)
			{
				Console.WriteLine("[combat-selftest] FAIL: no direction is walkable - map data may not be loading");
				return false;
			}

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

			AwardAndLevel(hero, 299, hero.PrimaryClass);
			ok &= CheckValue("still level 1", hero.TotalLevel, 1);

			AwardAndLevel(hero, 1, hero.PrimaryClass);
			ok &= CheckValue("level after 300 XP", hero.TotalLevel, 2);
			ok &= CheckValue("level 2 HP", hero.HitsMax, 14);
			ok &= CheckValue("level 2 slots", hero.GetMaxSpellSlots(1), 3);

			// One award crossing several thresholds at once must apply every level it earns.
			AwardAndLevel(hero, 6200, hero.PrimaryClass);
			ok &= CheckValue("level after 6500 XP", hero.TotalLevel, 5);
			ok &= CheckValue("level 5 proficiency", hero.PrimaryClass.GetProficiencyBonus(hero.TotalLevel), 3);
			ok &= CheckValue("level 5 3rd-level slots", hero.GetMaxSpellSlots(3), 2);
			ok &= CheckValue("level 5 cantrip dice", Spellcasting.GetCantripDice(hero.TotalLevel), 2);

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
				hero.TotalLevel,
				hero.Experience,
				hero.HitsMax,
				hero.PrimaryClass.GetProficiencyBonus(hero.TotalLevel));

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
