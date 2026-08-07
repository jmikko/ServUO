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
	}
}
