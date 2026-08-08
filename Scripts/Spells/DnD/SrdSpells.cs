using System;

namespace Server.Spells.DnD
{
	/// <summary>
	/// The opening set of SRD spells - one attack cantrip and one save cantrip, a reliable
	/// damage spell, and both healing options - chosen so that every resolution path
	/// (spell attack, saving throw, automatic) and both upcasting shapes are exercised by real
	/// content rather than by a test fixture.
	/// </summary>
	public static class SrdSpells
	{
		public static void Configure()
		{
			SpellRegistry.Register(new FireBolt(), "Sorcerer", "Wizard", "Warlock");
			SpellRegistry.Register(new SacredFlame(), "Cleric");
			SpellRegistry.Register(new MagicMissile(), "Sorcerer", "Wizard");
			SpellRegistry.Register(new CureWounds(), "Bard", "Cleric", "Druid", "Paladin", "Ranger");
			SpellRegistry.Register(new HealingWord(), "Bard", "Cleric", "Druid");

			Console.WriteLine("Spells: {0} SRD spell(s) registered.", SpellRegistry.Count);
		}
	}

	/// <summary>Cantrip. A ranged spell attack for 1d10 fire, scaling with character level.</summary>
	public sealed class FireBolt : DnDSpell
	{
		public override string Name { get { return "Fire Bolt"; } }
		public override int Level { get { return 0; } }
		public override SpellSchool School { get { return SpellSchool.Evocation; } }
		public override SpellResolution Resolution { get { return SpellResolution.SpellAttack; } }
		public override int Range { get { return 24; } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, int slotLevel)
		{
			int dice = Spellcasting.GetCantripDice(character.CharacterLevel);

			DnDCasting.ApplySpellDamage(
				caster, character, target, this, Utility.Dice(dice, 10, 0));
		}
	}

	/// <summary>Cantrip. The target makes a Dex save or takes 1d8 radiant; no damage on a success.</summary>
	public sealed class SacredFlame : DnDSpell
	{
		public override string Name { get { return "Sacred Flame"; } }
		public override int Level { get { return 0; } }
		public override SpellSchool School { get { return SpellSchool.Evocation; } }
		public override SpellResolution Resolution { get { return SpellResolution.SavingThrow; } }
		public override AbilityScoreType SaveAbility { get { return AbilityScoreType.Dex; } }
		public override int Range { get { return 24; } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, int slotLevel)
		{
			int dice = Spellcasting.GetCantripDice(character.CharacterLevel);

			DnDCasting.ApplySpellDamage(
				caster, character, target, this, Utility.Dice(dice, 8, 0));
		}
	}

	/// <summary>
	/// 1st level. Three darts, each 1d4+1 force, and they never miss - one more dart per slot
	/// level above 1st.
	/// </summary>
	public sealed class MagicMissile : DnDSpell
	{
		public override string Name { get { return "Magic Missile"; } }
		public override int Level { get { return 1; } }
		public override SpellSchool School { get { return SpellSchool.Evocation; } }
		public override SpellResolution Resolution { get { return SpellResolution.Automatic; } }
		public override int Range { get { return 24; } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, int slotLevel)
		{
			int darts = 3 + (slotLevel - Level);

			DnDCasting.ApplySpellDamage(
				caster, character, target, this, Utility.Dice(darts, 4, darts));
		}
	}

	/// <summary>1st level. Touch healing for 1d8 + spellcasting modifier, +1d8 per slot level above 1st.</summary>
	public sealed class CureWounds : DnDSpell
	{
		public override string Name { get { return "Cure Wounds"; } }
		public override int Level { get { return 1; } }
		public override SpellSchool School { get { return SpellSchool.Abjuration; } }
		public override bool Beneficial { get { return true; } }
		public override int Range { get { return 1; } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, int slotLevel)
		{
			int dice = 1 + (slotLevel - Level);
			int healed = Utility.Dice(dice, 8, Spellcasting.GetCastingAbilityModifier(character));

			Heal(target, healed);
		}

		/// <summary>
		/// Healing is a plain hit-point restore. Mobile.Hits clamps at HitsMax on its own, so this
		/// does not need to.
		/// </summary>
		internal static void Heal(Mobile target, int amount)
		{
			if (amount > 0 && target.Alive)
			{
				target.Hits += amount;
			}
		}
	}

	/// <summary>1st level. Cure Wounds at range for a smaller die - 1d4 + modifier.</summary>
	public sealed class HealingWord : DnDSpell
	{
		public override string Name { get { return "Healing Word"; } }
		public override int Level { get { return 1; } }
		public override SpellSchool School { get { return SpellSchool.Abjuration; } }
		public override bool Beneficial { get { return true; } }
		public override int Range { get { return 24; } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, int slotLevel)
		{
			int dice = 1 + (slotLevel - Level);

			CureWounds.Heal(target, Utility.Dice(dice, 4, Spellcasting.GetCastingAbilityModifier(character)));
		}
	}
}
