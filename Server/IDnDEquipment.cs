namespace Server
{
	/// <summary>
	/// Implemented by weapon/armor items that carry D&amp;D 5.5e proficiency-category and damage-dice data,
	/// used by CharacterClass.IsProficientWith for hard proficiency gating under Combat.RulesMode.
	/// </summary>
	public interface IDnDEquipment
	{
		WeaponCategory WeaponCategory { get; }
		ArmorCategory ArmorCategory { get; }

		/// <summary>
		/// For weapons: the damage dice expression, e.g. "1d8". For armor: the flat AC bonus this
		/// piece contributes (SRD-style base AC replacement/shield bonus), ignored for weapons.
		/// </summary>
		string DamageDiceExpression { get; }
		int ArmorBonus { get; }

		/// <summary>
		/// Whether a weapon may use the better of Strength or Dexterity for its attack and damage
		/// rolls. Armour always returns false. This is the SRD finesse property (used by daggers
		/// now and kept on the shared contract so later weapons do not need combat special cases).
		/// </summary>
		bool IsFinesse { get; }

		DnDDamageType DamageTypeDnD { get; }
	}
}
