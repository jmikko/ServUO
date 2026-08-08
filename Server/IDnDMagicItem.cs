using System;

namespace Server
{
	/// <summary>
	/// Represents a magical item in D&D 5.5e that provides bonuses.
	/// A character must be attuned to the item to receive its bonuses if it requires attunement.
	/// </summary>
	public interface IDnDMagicItem
	{
		bool RequiresAttunement { get; }

		int AttackBonus { get; }
		int DamageBonus { get; }
		int ArmorClassBonus { get; }
		int SavingThrowBonus { get; }

		/// <summary>
		/// Returns the ability score override, e.g. 19 for an Amulet of Health.
		/// Returns 0 if there is no override.
		/// </summary>
		int GetAbilityScoreOverride(AbilityScoreType type);
	}
}
