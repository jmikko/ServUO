using System;
using System.Collections.Generic;

namespace Server.Spells.DnD
{
	/// <summary>
	/// Tracks the one spell a caster is concentrating on.
	/// <para>
	/// Concentration is what stops a caster stacking every buff they own: starting a second
	/// concentration spell drops the first, and taking damage risks dropping it early. The save is
	/// DC 10 or half the damage taken, whichever is higher.
	/// </para>
	/// </summary>
	public static class DnDConcentration
	{
		public delegate void ConcentrationEnded(Mobile caster);

		private sealed class Entry
		{
			public string SpellName;
			public DateTime Expires;
			public ConcentrationEnded OnEnd;
		}

		private static readonly Dictionary<Mobile, Entry> m_Table = new Dictionary<Mobile, Entry>();

		public static string GetSpellName(Mobile caster)
		{
			Entry entry;

			return caster != null && m_Table.TryGetValue(caster, out entry) ? entry.SpellName : null;
		}

		public static bool IsConcentrating(Mobile caster)
		{
			return GetSpellName(caster) != null;
		}

		/// <summary>
		/// Begins concentrating, ending whatever was being concentrated on before. A caster only
		/// ever holds one.
		/// </summary>
		public static void Begin(Mobile caster, string spellName, TimeSpan duration, ConcentrationEnded onEnd)
		{
			if (caster == null)
			{
				return;
			}

			End(caster, "You stop concentrating on {0}.");

			m_Table[caster] = new Entry
			{
				SpellName = spellName,
				Expires = duration == TimeSpan.Zero ? DateTime.MaxValue : DateTime.UtcNow + duration,
				OnEnd = onEnd
			};
		}

		public static void End(Mobile caster, string messageFormat = null)
		{
			Entry entry;

			if (caster == null || !m_Table.TryGetValue(caster, out entry))
			{
				return;
			}

			m_Table.Remove(caster);

			if (entry.OnEnd != null)
			{
				entry.OnEnd(caster);
			}

			if (messageFormat != null)
			{
				caster.SendMessage(messageFormat, entry.SpellName);
			}
		}

		/// <summary>
		/// Called when a concentrating caster takes damage. SRD: a Constitution saving throw
		/// against DC 10 or half the damage, whichever is higher.
		/// </summary>
		public static void OnDamaged(Mobile caster, int damage)
		{
			Entry entry;

			if (caster == null || damage <= 0 || !m_Table.TryGetValue(caster, out entry))
			{
				return;
			}

			if (DateTime.UtcNow >= entry.Expires)
			{
				End(caster, "{0} fades.");
				return;
			}

			int dc = Math.Max(10, damage / 2);

			if (!CombatRules.CheckSave(caster, AbilityScoreType.Con, dc))
			{
				End(caster, "You lose concentration on {0}.");
			}
		}

		/// <summary>Drops concentration that has simply run out of time.</summary>
		public static void CheckExpiry(Mobile caster)
		{
			Entry entry;

			if (caster != null && m_Table.TryGetValue(caster, out entry) && DateTime.UtcNow >= entry.Expires)
			{
				End(caster, "{0} fades.");
			}
		}
	}
}
