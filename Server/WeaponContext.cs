using System;

namespace Server
{
	/// <summary>
	/// What a rule needs to know about the weapon being swung.
	/// <para>
	/// This exists because a whole class of features - Archery, Sharpshooter, Great Weapon Master,
	/// Duelling, Two-Weapon Fighting - are conditional on the weapon, and the hooks that fed them
	/// were not handed one. The result was a set of unconditional bonuses wearing conditional
	/// names: Archery's +2 to ranged attacks applied to every attack, including melee.
	/// </para>
	/// <para>
	/// Passed by value as a small struct rather than the item itself, so <c>Server</c> rules can
	/// read it without needing the concrete weapon types that live in <c>Scripts</c>.
	/// </para>
	/// </summary>
	public struct WeaponContext
	{
		public bool Ranged;
		public bool Finesse;
		public bool Heavy;
		public bool Light;
		public bool TwoHanded;

		/// <summary>True when nothing is wielded in the off hand - what Duelling requires.</summary>
		public bool OffHandFree;

		/// <summary>Unarmed, or wielding nothing the equipment tables know about.</summary>
		public bool Unarmed;

		public static WeaponContext For(Mobile attacker, IDnDEquipment weapon)
		{
			var context = new WeaponContext();

			if (weapon == null)
			{
				context.Unarmed = true;
				context.OffHandFree = true;

				return context;
			}

			context.Ranged = weapon.WeaponCategory == WeaponCategory.SimpleRanged
						  || weapon.WeaponCategory == WeaponCategory.MartialRanged;

			context.Finesse = weapon.IsFinesse;

			var properties = weapon as IDnDWeaponProperties;

			if (properties != null)
			{
				context.Heavy = (properties.Properties & WeaponProperty.Heavy) != 0;
				context.Light = (properties.Properties & WeaponProperty.Light) != 0;
				context.TwoHanded = (properties.Properties & WeaponProperty.TwoHanded) != 0;
			}

			// Two-handed weapons occupy both hands by definition; otherwise ask the paperdoll.
			context.OffHandFree = !context.TwoHanded
				&& (attacker == null || attacker.FindItemOnLayer(Layer.TwoHanded) == null);

			return context;
		}
	}

	/// <summary>
	/// The SRD weapon properties, surfaced separately from <see cref="IDnDEquipment"/> so that
	/// interface does not have to change for every rule that wants to read one.
	/// </summary>
	public interface IDnDWeaponProperties
	{
		WeaponProperty Properties { get; }
	}
}
