using System;

namespace Server
{
	[Flags]
	public enum WeaponCategory
	{
		None = 0x0,
		SimpleMelee = 0x1,
		SimpleRanged = 0x2,
		MartialMelee = 0x4,
		MartialRanged = 0x8,

		AllSimple = SimpleMelee | SimpleRanged,
		AllMartial = MartialMelee | MartialRanged,
		All = AllSimple | AllMartial
	}
}
