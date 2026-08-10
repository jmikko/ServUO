using System;

namespace Server
{
	public enum DnDActionType
	{
		MeleeWeapon,
		RangedWeapon,
		MeleeSpell,
		RangedSpell,
		BreathWeapon,
		Utility
	}

	public enum DnDUsageType
	{
		AtWill,
		Recharge5_6,
		Recharge6,
		PerDay1,
		PerDay2,
		PerDay3
	}

	public class DnDAction
	{
		public string Name { get; set; }
		public DnDActionType Type { get; set; }
		public int ToHit { get; set; }
		public string ReachOrRange { get; set; }
		public string PrimaryDamageDice { get; set; }
		public DnDDamageType PrimaryDamageType { get; set; }
		public string SecondaryDamageDice { get; set; }
		public DnDDamageType SecondaryDamageType { get; set; }
		public DnDUsageType Usage { get; set; }
		public string SpecialEffect { get; set; }

		public DnDAction()
		{
			Name = "Action";
			Type = DnDActionType.MeleeWeapon;
			ToHit = 0;
			ReachOrRange = "5 ft.";
			PrimaryDamageDice = "1d4";
			PrimaryDamageType = DnDDamageType.Bludgeoning;
			SecondaryDamageDice = string.Empty;
			SecondaryDamageType = DnDDamageType.None;
			Usage = DnDUsageType.AtWill;
			SpecialEffect = string.Empty;
		}
	}

	public class DnDLegendaryAction
	{
		public string Name { get; set; }
		public int Cost { get; set; }
		public string Description { get; set; }

		public DnDLegendaryAction()
		{
			Name = "Legendary Action";
			Cost = 1;
			Description = string.Empty;
		}
	}
}
