namespace Server.Items
{
	/// <summary>SRD dagger: simple melee, 1d4 piercing, finesse.</summary>
	public sealed class DnDDagger : DnDWeapon
	{
		public override WeaponCategory WeaponCategory { get { return WeaponCategory.SimpleMelee; } }
		public override string DamageDiceExpression { get { return "1d4"; } }
		public override bool IsFinesse { get { return true; } }

		[Constructable]
		public DnDDagger() : base(0x0F52)
		{
			Name = "a dagger";
			Weight = 1.0;
		}

		public DnDDagger(Serial serial) : base(serial) { }
	}

	/// <summary>SRD quarterstaff: simple melee, 1d6 bludgeoning, two-handed.</summary>
	public sealed class DnDQuarterstaff : DnDWeapon
	{
		public override WeaponCategory WeaponCategory { get { return WeaponCategory.SimpleMelee; } }
		public override string DamageDiceExpression { get { return "1d6"; } }

		[Constructable]
		public DnDQuarterstaff() : base(0x0E89, Layer.TwoHanded)
		{
			Name = "a quarterstaff";
			Weight = 4.0;
		}

		public DnDQuarterstaff(Serial serial) : base(serial) { }
	}

	/// <summary>SRD shortbow: simple ranged, 1d6 piercing, two-handed.</summary>
	public sealed class DnDShortbow : DnDWeapon
	{
		public override WeaponCategory WeaponCategory { get { return WeaponCategory.SimpleRanged; } }
		public override string DamageDiceExpression { get { return "1d6"; } }

		[Constructable]
		public DnDShortbow() : base(0x13B2, Layer.TwoHanded)
		{
			Name = "a shortbow";
			Weight = 2.0;
		}

		public DnDShortbow(Serial serial) : base(serial) { }
	}
}
