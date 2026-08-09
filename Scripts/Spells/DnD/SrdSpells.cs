using System;

namespace Server.Spells.DnD
{
	/// <summary>
	/// Hand-written spells - the ones whose behaviour cannot be described as a row of dice in
	/// Data/DnDSpells.xml.
	/// <para>
	/// This file is deliberately empty for now: every spell implemented so far fits the data table,
	/// and the five that used to live here as classes (Fire Bolt, Sacred Flame, Magic Missile, Cure
	/// Wounds, Healing Word) moved into it unchanged. The extension point stays because area
	/// effects, conditions and concentration will need real code.
	/// </para>
	/// </summary>
	public static class SrdSpells
	{
		public static void Configure()
		{
			// Registration order determines spell ids on the wire, and DnDSpellTable.Configure has
			// already run by the time this does, so anything registered here lands after the data
			// table - appending, never renumbering it.
			
			SpellRegistry.Register(new HuntersMarkSpell(), "Ranger");
			SpellRegistry.Register(new HasteSpell(), "Sorcerer", "Wizard");
			SpellRegistry.Register(new PassWithoutTraceSpell(), "Druid", "Ranger");
			SpellRegistry.Register(new MagicWeaponSpell(), "Paladin", "Wizard");
		}
	}
	
	public sealed class HuntersMarkSpell : DnDSpell
	{
		public override string Name { get { return "Hunter's Mark"; } }
		public override int Level { get { return 1; } }
		public override SpellSchool School { get { return SpellSchool.Divination; } }
		public override SpellResolution Resolution { get { return SpellResolution.Automatic; } }
		public override AbilityScoreType SaveAbility { get { return AbilityScoreType.Str; } }
		public override int Range { get { return 18; } }
		public override bool HalfDamageOnSave { get { return false; } }
		public override SpellTargetType TargetType { get { return SpellTargetType.Mobile; } }
		public override bool Beneficial { get { return false; } }
		public override SpellShape Shape { get { return SpellShape.Single; } }
		public override int AreaSize { get { return 0; } }
		public override bool RequiresConcentration { get { return true; } }
		public override TimeSpan Duration { get { return TimeSpan.FromHours(1); } } // Scales with slot level in 5e, simplified to 1h

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, Point3D targetLocation, int slotLevel)
		{
			DnDEffects.ApplyHuntersMark(caster, target, Duration);
			caster.SendMessage("You mark {0} as your quarry.", target.Name);
		}
	}
	
	public sealed class HasteSpell : DnDSpell
	{
		public override string Name { get { return "Haste"; } }
		public override int Level { get { return 3; } }
		public override SpellSchool School { get { return SpellSchool.Transmutation; } }
		public override SpellResolution Resolution { get { return SpellResolution.Automatic; } }
		public override AbilityScoreType SaveAbility { get { return AbilityScoreType.Str; } }
		public override int Range { get { return 6; } }
		public override bool HalfDamageOnSave { get { return false; } }
		public override SpellTargetType TargetType { get { return SpellTargetType.Mobile; } }
		public override bool Beneficial { get { return true; } }
		public override SpellShape Shape { get { return SpellShape.Single; } }
		public override int AreaSize { get { return 0; } }
		public override bool RequiresConcentration { get { return true; } }
		public override TimeSpan Duration { get { return TimeSpan.FromMinutes(1); } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, Point3D targetLocation, int slotLevel)
		{
			// +2 AC
			// Advantage on Dex saves
			// We can implement AC bonus using DnDEffects.ApplyArmorClassBonus if it exists, or just use RollModifier?
			// Actually, AC isn't a RollModifier. Let's add ApplyArmorClassBonus to DnDEffects!
			DnDEffects.ApplyArmorClassBonus(target, 2, Duration, Name);
			DnDRollModifiers.AddAdvantage(target, RollKind.Save, Duration, Name); // We should ideally limit to Dex save, but HasAdvantage currently applies to all saves in CombatRules.cs!
			
			target.SendMessage("Your movements accelerate!");
		}
	}
	
	public sealed class PassWithoutTraceSpell : DnDSpell
	{
		public override string Name { get { return "Pass without Trace"; } }
		public override int Level { get { return 2; } }
		public override SpellSchool School { get { return SpellSchool.Abjuration; } }
		public override SpellResolution Resolution { get { return SpellResolution.Automatic; } }
		public override AbilityScoreType SaveAbility { get { return AbilityScoreType.Str; } }
		public override int Range { get { return 0; } }
		public override bool HalfDamageOnSave { get { return false; } }
		public override SpellTargetType TargetType { get { return SpellTargetType.Mobile; } }
		public override bool Beneficial { get { return true; } }
		public override SpellShape Shape { get { return SpellShape.Single; } }
		public override int AreaSize { get { return 0; } }
		public override bool RequiresConcentration { get { return true; } }
		public override TimeSpan Duration { get { return TimeSpan.FromHours(1); } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, Point3D targetLocation, int slotLevel)
		{
			DnDRollModifiers.Add(caster, Name, 0, 10, 1, RollKind.AbilityCheck, Duration, false);
			caster.SendMessage("Shadows and silence radiate from you.");
		}
	}
	
	public sealed class MagicWeaponSpell : DnDSpell
	{
		public override string Name { get { return "Magic Weapon"; } }
		public override int Level { get { return 2; } }
		public override SpellSchool School { get { return SpellSchool.Transmutation; } }
		public override SpellResolution Resolution { get { return SpellResolution.Automatic; } }
		public override AbilityScoreType SaveAbility { get { return AbilityScoreType.Str; } }
		public override int Range { get { return 1; } }
		public override bool HalfDamageOnSave { get { return false; } }
		public override SpellTargetType TargetType { get { return SpellTargetType.Mobile; } } // We'll target the wielder for simplicity since UO target types are restrictive
		public override bool Beneficial { get { return true; } }
		public override SpellShape Shape { get { return SpellShape.Single; } }
		public override int AreaSize { get { return 0; } }
		public override bool RequiresConcentration { get { return true; } }
		public override TimeSpan Duration { get { return TimeSpan.FromHours(1); } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, Point3D targetLocation, int slotLevel)
		{
			int bonus = slotLevel >= 6 ? 3 : (slotLevel >= 4 ? 2 : 1);
			DnDRollModifiers.Add(target, Name, 0, bonus, 1, RollKind.Attack | RollKind.Damage, Duration, false);
			target.SendMessage("Your weapon is imbued with magic!");
		}
	}
}
