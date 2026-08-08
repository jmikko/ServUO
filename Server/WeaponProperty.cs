using System;

namespace Server
{
	/// <summary>
	/// The SRD weapon properties. Flags rather than separate fields because a weapon routinely has
	/// several - a spear is Thrown and Versatile, a shortsword is Finesse and Light.
	/// </summary>
	[Flags]
	public enum WeaponProperty
	{
		None = 0x000,

		/// <summary>Attack and damage may use Dexterity instead of Strength.</summary>
		Finesse = 0x001,

		/// <summary>Small enough for two-weapon fighting.</summary>
		Light = 0x002,

		/// <summary>Small creatures have disadvantage attacking with it.</summary>
		Heavy = 0x004,

		TwoHanded = 0x008,

		/// <summary>Can be thrown to make a ranged attack.</summary>
		Thrown = 0x010,

		/// <summary>Adds 5 feet to the attack's reach.</summary>
		Reach = 0x020,

		/// <summary>Needs ammunition to fire.</summary>
		Ammunition = 0x040,

		/// <summary>Only one shot per action, however many attacks are available.</summary>
		Loading = 0x080,

		/// <summary>Can be used one- or two-handed, with a larger die two-handed.</summary>
		Versatile = 0x100,

		/// <summary>Carries a rule of its own (the lance and the net).</summary>
		Special = 0x200
	}

	/// <summary>
	/// SRD physical damage types. Kept separate from the dice so resistances and immunities have
	/// something to key off when creatures grow them.
	/// </summary>
	public enum DamageType
	{
		Bludgeoning,
		Piercing,
		Slashing
	}
}
