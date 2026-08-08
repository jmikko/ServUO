using System;
using Server.Network;

namespace Server.Misc
{
	/// <summary>
	/// Sets <see cref="Core.Expansion"/> and the handful of engine feature flags that hang off it.
	/// <para>
	/// Expansion still matters on a D&amp;D shard even though none of UO's expansion content
	/// survives, because it gates what the *client* has art and protocol support for. Left unset it
	/// defaults to <see cref="Expansion.None"/>, which silently makes every species that needs
	/// later art - Elf, Gargoyle - fail its availability check.
	/// </para>
	/// <para>
	/// Deliberately a small replacement rather than a port of the stock CurrentExpansion.cs, which
	/// also configures account gold, virtual checks, insurance, Siege rules and the AOS status
	/// handler - all systems that do not exist here.
	/// </para>
	/// </summary>
	public class ExpansionConfig
	{
		public static readonly Expansion Expansion =
			Config.GetEnum("Expansion.CurrentExpansion", Expansion.EJ);

		[CallPriority(Int32.MinValue)]
		public static void Configure()
		{
			Core.Expansion = Expansion;

			// Item tooltips. DnDWeapon/DnDArmor report their dice and armour class through
			// GetProperties, which only reaches the client when this is on.
			ObjectPropertyList.Enabled = Core.AOS;

			if (ObjectPropertyList.Enabled)
			{
				PacketHandlers.SingleClickProps = true;
			}

			// Floating damage numbers - worth having when every hit is a dice roll.
			Mobile.VisibleDamageType = Core.AOS ? VisibleDamageType.Related : VisibleDamageType.None;

			Mobile.GuildClickMessage = !Core.AOS;
			Mobile.AsciiClickMessage = !Core.AOS;

			Mobile.ActionDelay = Core.TOL ? 500 : Core.AOS ? 1000 : 500;

			Console.WriteLine("Expansion: {0}", Core.Expansion);
		}
	}
}
