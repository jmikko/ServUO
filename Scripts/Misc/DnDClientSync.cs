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

					pm.NetState.Send(new DnDStatSync(pm));

					DnDSpellPackets.SendSpellList(pm);
				});
		}
	}
}
