using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;

namespace Server.Spells.DnD
{
	/// <summary>
	/// The client-facing half of spellcasting: sends a character their castable spells and slot
	/// counts, and turns their cast requests into actual casts.
	/// <para>
	/// The client is never trusted to know whether a cast is legal - it sends an id and a target,
	/// and <see cref="DnDCasting.Cast"/> decides. The spell list it receives is only ever a
	/// convenience for rendering; a client that asks for a spell it was not sent is refused by the
	/// same rules as any other bad request.
	/// </para>
	/// </summary>
	public static class DnDSpellPackets
	{
		public static void Initialize()
		{
			EventSink.DnDCastRequest += OnCastRequest;
		}

		private static void OnCastRequest(DnDCastRequestEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (pm == null)
			{
				return;
			}

			DnDSpell spell = SpellRegistry.FindById(e.SpellId);

			if (spell == null)
			{
				Send(pm, new DnDCastResult(e.SpellId, (int)CastResult.NotOnClassList));
				return;
			}

			Mobile target = e.TargetSerial == Serial.Zero ? pm : World.FindMobile(e.TargetSerial);

			CastResult result = DnDCasting.Cast(pm, spell, target);

			pm.SendMessage(DescribeResult(spell, result));

			Send(pm, new DnDCastResult(e.SpellId, (int)result));

			// The client cannot recompute slots or hit points itself, so refresh both after any
			// cast - including a failed one, whose whole point may be that a slot was missing.
			SendSpellList(pm);
			Send(pm, new DnDStatSync(pm));
		}

		/// <summary>
		/// Sends the character's castable spells and current slot counts. Safe to call for
		/// non-casters - they simply receive an empty list.
		/// </summary>
		public static void SendSpellList(DnDPlayerMobile pm)
		{
			if (pm == null || pm.NetState == null || !pm.DnDInitialized)
			{
				return;
			}

			var infos = new List<DnDSpellInfo>();

			foreach (DnDSpell spell in SpellRegistry.GetAvailable(pm))
			{
				infos.Add(new DnDSpellInfo(SpellRegistry.GetId(spell), spell.Level, spell.School, spell.Name));
			}

			var available = new int[Spellcasting.MaxSpellLevel];
			var max = new int[Spellcasting.MaxSpellLevel];

			for (int level = 1; level <= Spellcasting.MaxSpellLevel; ++level)
			{
				available[level - 1] = pm.GetAvailableSpellSlots(level);
				max[level - 1] = pm.GetMaxSpellSlots(level);
			}

			pm.NetState.Send(new DnDSpellList(infos, available, max));
		}

		private static void Send(DnDPlayerMobile pm, Packet packet)
		{
			if (pm.NetState != null)
			{
				pm.NetState.Send(packet);
			}
		}

		private static string DescribeResult(DnDSpell spell, CastResult result)
		{
			switch (result)
			{
				case CastResult.Success:
					return String.Format("You cast {0}.", spell.Name);
				case CastResult.NotACaster:
					return "You cannot cast spells.";
				case CastResult.NotOnClassList:
					return String.Format("{0} is not on your spell list.", spell.Name);
				case CastResult.NoSlotAvailable:
					return String.Format("You have no spell slot left for {0}.", spell.Name);
				case CastResult.NoTarget:
					return "You need a living target.";
				case CastResult.OutOfRange:
					return "That is too far away.";
				case CastResult.WrongTargetType:
					return "You cannot target yourself with that.";
			}

			return "Nothing happens.";
		}
	}
}
