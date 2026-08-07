namespace Server
{
	/// <summary>
	/// Implemented by Race subclasses to carry D&amp;D 5.5e species traits (ability score bonuses,
	/// darkvision). Kept intentionally small - size/speed and more elaborate SRD traits (breath
	/// weapons, resistances, Lucky rerolls) are out of scope for this pass; see individual Race
	/// implementations for the specific bonuses each species grants.
	/// </summary>
	public interface IDnDSpecies
	{
		int GetAbilityScoreBonus(AbilityScoreType type);
		bool HasDarkvision { get; }
	}
}
