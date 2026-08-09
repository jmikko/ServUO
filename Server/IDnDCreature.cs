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

	/// <summary>
	/// Implemented by anything carrying a monster trait block.
	/// <para>
	/// Separate from <see cref="IDnDCreature"/> and living in Server/ because the rules that read
	/// it - the attack roll, the condition table - are engine code and cannot see Scripts/, where
	/// the monsters themselves are defined. Without this the traits would be data the rules could
	/// not reach, which is the same as no traits at all.
	/// </para>
	/// </summary>
	public interface IDnDTraited
	{
		DnDMonsterTraits Traits { get; }
	}
}
