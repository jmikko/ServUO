using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells.DnD;

namespace Server.Misc
{
	/// <summary>
	/// Brings a client's view of a D&amp;D character up to date when they log in.
	/// <para>
	/// Without this, the character sheet and spellbook only ever appeared as a side effect of
	/// something changing - setup, a level-up, a cast. A returning player got neither, and the
	/// spellbook in particular would stay missing for an entire session.
	/// </para>
	/// </summary>
	public static class DnDClientSync
	{
		public static void Initialize()
		{
			EventSink.Login += OnLogin;
		}

		/// <summary>
		/// Pushes the whole character sheet - stats and skills - to a player's client.
		/// <para>
		/// One call rather than two at each of a dozen sites, because the two halves go stale
		/// together and a site that sends one without the other is a bug nobody sees: the sheet
		/// keeps showing the old numbers and looks merely out of date rather than broken. Levelling
		/// up is the case that matters, since a new proficiency bonus moves every proficient skill
		/// at once.
		/// </para>
		/// <para>
		/// Safe to call on a character with no client attached; it does nothing.
		/// </para>
		/// </summary>
		public static void SendSheet(Mobile m)
		{
			if (m == null || m.NetState == null)
			{
				return;
			}

			m.NetState.Send(new DnDStatSync(m));
			m.NetState.Send(new DnDSkillSync(m));
		}

		private static void OnLogin(LoginEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (pm == null || !pm.DnDInitialized)
			{
				return;
			}

			// Deliberately delayed, for the same reason DnDCharacterCreation delays its prompt:
			// Login fires in the middle of PacketHandlers.DoLogin's world-entry burst, before
			// SendEverything, and a gump added while the client is still switching scenes is
			// dropped without a trace.
			Timer.DelayCall(
				TimeSpan.FromSeconds(1.0),
				() =>
				{
					if (pm.NetState == null || !pm.NetState.Running)
					{
						return;
					}

					SendSheet(pm);

					DnDSpellPackets.SendSpellList(pm);
				});
		}
	}
}
