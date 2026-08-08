namespace Server.Items
{
	/// <summary>SRD longsword: martial melee, 1d8 slashing.</summary>
	public class DnDLongsword : DnDWeapon
	{
		public override WeaponCategory WeaponCategory { get { return WeaponCategory.MartialMelee; } }
		public override string DamageDiceExpression { get { return "1d8"; } }

		[Constructable]
		public DnDLongsword()
			: base(0x0F5E)
		{
			Name = "a longsword";
			Weight = 3.0;
		}

		public DnDLongsword(Serial serial)
			: base(serial)
		{
		}
	}
}
