using System;

namespace Server
{
	[Flags]
	public enum ArmorCategory
	{
		None = 0x0,
		Light = 0x1,
		Medium = 0x2,
		Heavy = 0x4,
		Shield = 0x8,

		All = Light | Medium | Heavy | Shield
	}
}
