namespace Server
{
	/// <summary>
	/// Implemented by BaseCreature subclasses that carry D&amp;D 5.5e stat block data (AC/HP/damage
	/// dice), consumed unconditionally by BaseWeapon's combat resolution (see GetDnDArmorClass/
	/// GetDnDAttackBonus). Creatures that don't implement this fall back to AC 10 / +0 attack bonus
	/// until converted.
	/// </summary>
	public interface IDnDCreature
	{
		int ArmorClass { get; }
		int AttackBonus { get; }
		string DamageDiceExpression { get; }
		int HitPointsMaxDnD { get; }
	}
}
