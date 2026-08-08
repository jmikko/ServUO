using System;

namespace Server.Engines.Classes.Features
{
	// ============================ Defence ============================

	/// <summary>
	/// SRD Unarmored Defense. The Barbarian's is 10 + Dex + Con, the Monk's 10 + Dex + Wis, and
	/// both apply only with no armour worn - which is why this replaces the armour class outright
	/// rather than adding to it.
	/// </summary>
	public sealed class BarbarianUnarmoredDefenseFeature : ClassFeature
	{
		public override string Name { get { return "Unarmored Defense"; } }
		public override int Level { get { return 1; } }
		public override string Description { get { return "Unarmoured, your armour class is 10 + Dexterity + Constitution."; } }

		public override int GetUnarmoredArmorClass(IDnDCharacter character, int classLevel)
		{
			AbilityScores scores = character.EffectiveAbilityScores;

			return 10 + scores.DexMod + scores.ConMod;
		}
	}

	public sealed class MonkUnarmoredDefenseFeature : ClassFeature
	{
		public override string Name { get { return "Unarmored Defense"; } }
		public override int Level { get { return 1; } }
		public override string Description { get { return "Unarmoured, your armour class is 10 + Dexterity + Wisdom."; } }

		public override int GetUnarmoredArmorClass(IDnDCharacter character, int classLevel)
		{
			AbilityScores scores = character.EffectiveAbilityScores;

			return 10 + scores.DexMod + scores.WisMod;
		}
	}

	/// <summary>
	/// SRD Defense fighting style: +1 armour class while wearing armour.
	/// <para>
	/// Not granted automatically. Fighting styles are a choice a Fighter, Paladin or Ranger makes,
	/// and handing every one of them Defense would quietly raise the armour class of every martial
	/// character in the game. It waits here for a style-choice step alongside skills and feats.
	/// </para>
	/// </summary>
	public sealed class DefenseStyleFeature : ClassFeature
	{
		public override string Name { get { return "Fighting Style: Defense"; } }
		public override int Level { get { return 1; } }
		public override int ArmorClassBonus { get { return 1; } }
		public override string Description { get { return "+1 armour class while you wear armour."; } }
	}

	/// <summary>SRD Archery fighting style: +2 to ranged attack rolls.</summary>
	public sealed class ArcheryStyleFeature : ClassFeature
	{
		public override string Name { get { return "Fighting Style: Archery"; } }
		public override int Level { get { return 2; } }
		public override int AttackBonus { get { return 2; } }
		public override string Description { get { return "+2 to attack rolls with ranged weapons."; } }
	}

	// ============================ Saving throws ============================

	/// <summary>SRD Danger Sense: advantage on Dexterity saves against things you can see.</summary>
	public sealed class DangerSenseFeature : ClassFeature
	{
		public override string Name { get { return "Danger Sense"; } }
		public override int Level { get { return 2; } }
		public override string Description { get { return "Advantage on Dexterity saving throws."; } }

		public override bool GrantsSaveAdvantage(AbilityScoreType ability)
		{
			return ability == AbilityScoreType.Dex;
		}
	}

	/// <summary>SRD Aura of Protection: you add your Charisma modifier to every saving throw.</summary>
	public sealed class AuraOfProtectionFeature : ClassFeature
	{
		public override string Name { get { return "Aura of Protection"; } }
		public override int Level { get { return 6; } }
		public override string Description { get { return "Add your Charisma modifier to saving throws."; } }

		public override int GetSaveBonus(IDnDCharacter character, int classLevel)
		{
			return Math.Max(1, character.EffectiveAbilityScores.ChaMod);
		}
	}

	/// <summary>SRD Diamond Soul: a Monk becomes proficient in every saving throw.</summary>
	public sealed class DiamondSoulFeature : ClassFeature
	{
		public override string Name { get { return "Diamond Soul"; } }
		public override int Level { get { return 14; } }
		public override string Description { get { return "Proficiency in all saving throws."; } }

		public override int GetSaveBonus(IDnDCharacter character, int classLevel)
		{
			// Proficiency in everything, expressed as the bonus it would add.
			return character.PrimaryClass == null ? 0 : character.PrimaryClass.GetProficiencyBonus(character.TotalLevel);
		}
	}

	/// <summary>SRD Slippery Mind: a Rogue gains proficiency in Wisdom saves.</summary>
	public sealed class SlipperyMindFeature : ClassFeature
	{
		public override string Name { get { return "Slippery Mind"; } }
		public override int Level { get { return 15; } }
		public override string Description { get { return "Advantage on Wisdom saving throws."; } }

		public override bool GrantsSaveAdvantage(AbilityScoreType ability)
		{
			return ability == AbilityScoreType.Wis;
		}
	}

	/// <summary>SRD Magic Resistance, the Monk's Empty Body-adjacent late defence.</summary>
	public sealed class IndomitableFeature : ClassFeature
	{
		public override string Name { get { return "Indomitable"; } }
		public override int Level { get { return 9; } }
		public override string Description { get { return "Advantage on Wisdom and Charisma saving throws."; } }

		public override bool GrantsSaveAdvantage(AbilityScoreType ability)
		{
			return ability == AbilityScoreType.Wis || ability == AbilityScoreType.Cha;
		}
	}

	// ============================ Damage ============================

	/// <summary>
	/// SRD Brutal Critical: extra weapon dice on a critical hit, one more at 13th and 17th.
	/// </summary>
	public sealed class BrutalCriticalFeature : ClassFeature
	{
		public override string Name { get { return "Brutal Critical"; } }
		public override int Level { get { return 9; } }
		public override string Description { get { return "Extra weapon dice on a critical hit."; } }

		public override int ExtraCriticalDice(int classLevel)
		{
			if (classLevel >= 17) { return 3; }
			if (classLevel >= 13) { return 2; }

			return classLevel >= 9 ? 1 : 0;
		}
	}

	/// <summary>
	/// SRD Martial Arts, reduced to what is expressible: a Monk's unarmed strikes hit harder as
	/// they level. The full feature also changes attack ability and grants a bonus strike.
	/// </summary>
	public sealed class MartialArtsFeature : ClassFeature
	{
		public override string Name { get { return "Martial Arts"; } }
		public override int Level { get { return 1; } }
		public override string Description { get { return "Your unarmed strikes deal a martial arts die."; } }

		public override string GetBonusDamage(IDnDCharacter character, int classLevel, RollMode mode)
		{
			if (classLevel >= 17) { return "1d10"; }
			if (classLevel >= 11) { return "1d8"; }
			if (classLevel >= 5) { return "1d6"; }

			return "1d4";
		}
	}

	/// <summary>
	/// SRD Improved Divine Smite: a Paladin's weapon hits carry radiant damage from 11th level.
	/// </summary>
	public sealed class ImprovedDivineSmiteFeature : ClassFeature
	{
		public override string Name { get { return "Improved Divine Smite"; } }
		public override int Level { get { return 11; } }
		public override string Description { get { return "Your weapon hits deal an extra 1d8 radiant."; } }

		public override string GetBonusDamage(IDnDCharacter character, int classLevel, RollMode mode)
		{
			return "1d8";
		}
	}

	/// <summary>
	/// SRD Foe Slayer: a Ranger adds their Wisdom modifier to one attack's damage each turn.
	/// Modelled as a standing bonus, since there are no turns to spend it on.
	/// </summary>
	public sealed class FoeSlayerFeature : ClassFeature
	{
		public override string Name { get { return "Foe Slayer"; } }
		public override int Level { get { return 20; } }
		public override string Description { get { return "Add your Wisdom modifier to weapon damage."; } }

		public override string GetBonusDamage(IDnDCharacter character, int classLevel, RollMode mode)
		{
			int bonus = character.EffectiveAbilityScores.WisMod;

			return bonus > 0 ? "1d1+" + (bonus - 1) : null;
		}
	}

	// ============================ Extra attacks ============================

	/// <summary>
	/// SRD Extra Attack at 11th and 20th for Fighters, who attack three and then four times.
	/// </summary>
	public sealed class FighterExtraAttackFeature : ClassFeature
	{
		private readonly int m_Level;
		private readonly int m_Extra;

		public FighterExtraAttackFeature(int level, int extra)
		{
			m_Level = level;
			m_Extra = extra;
		}

		public override string Name { get { return "Extra Attack"; } }
		public override int Level { get { return m_Level; } }
		public override int ExtraAttacks { get { return m_Extra; } }

		public override string Description
		{
			get { return String.Format("You attack {0} times whenever you take the Attack action.", m_Extra + 1); }
		}
	}

	// ============================ Late-game ability growth ============================

	/// <summary>
	/// SRD Primal Champion: a 20th-level Barbarian's Strength and Constitution rise by 4, past
	/// the usual cap of 20.
	/// </summary>
	public sealed class PrimalChampionFeature : ClassFeature
	{
		public override string Name { get { return "Primal Champion"; } }
		public override int Level { get { return 20; } }
		public override string Description { get { return "Your Strength and Constitution increase by 4."; } }

		public override int GetUnarmoredArmorClass(IDnDCharacter character, int classLevel)
		{
			// The Constitution rise shows up in unarmoured defence, which is where a Barbarian
			// feels it. The score change itself needs an ability-override system.
			AbilityScores scores = character.EffectiveAbilityScores;

			return 10 + scores.DexMod + scores.ConMod + 2;
		}
	}
}
