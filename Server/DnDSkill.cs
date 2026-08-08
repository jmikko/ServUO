using System;

namespace Server
{
	/// <summary>
	/// The 18 standard D&amp;D 5.5e skills.
	/// </summary>
	public enum DnDSkill
	{
		Acrobatics,
		AnimalHandling,
		Arcana,
		Athletics,
		Deception,
		History,
		Insight,
		Intimidation,
		Investigation,
		Medicine,
		Nature,
		Perception,
		Performance,
		Persuasion,
		Religion,
		SleightOfHand,
		Stealth,
		Survival
	}

	public static class DnDSkills
	{
		public static AbilityScoreType GetPrimaryAbility(DnDSkill skill)
		{
			switch (skill)
			{
				case DnDSkill.Athletics:
					return AbilityScoreType.Str;

				case DnDSkill.Acrobatics:
				case DnDSkill.SleightOfHand:
				case DnDSkill.Stealth:
					return AbilityScoreType.Dex;

				case DnDSkill.Arcana:
				case DnDSkill.History:
				case DnDSkill.Investigation:
				case DnDSkill.Nature:
				case DnDSkill.Religion:
					return AbilityScoreType.Int;

				case DnDSkill.AnimalHandling:
				case DnDSkill.Insight:
				case DnDSkill.Medicine:
				case DnDSkill.Perception:
				case DnDSkill.Survival:
					return AbilityScoreType.Wis;

				case DnDSkill.Deception:
				case DnDSkill.Intimidation:
				case DnDSkill.Performance:
				case DnDSkill.Persuasion:
					return AbilityScoreType.Cha;

				default:
					return AbilityScoreType.Int;
			}
		}
	}
}
