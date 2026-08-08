#region References
using System;
using System.Collections.Generic;
#endregion

namespace Server
{
	/// <summary>
	/// Which kinds of d20 roll a modifier applies to. Flags because Bless touches two of them.
	/// </summary>
	[Flags]
	public enum RollKind
	{
		None = 0x0,
		Attack = 0x1,
		Save = 0x2,
		AbilityCheck = 0x4
	}

	/// <summary>
	/// Temporary dice added to or subtracted from d20 rolls - the Bless and Bane family.
	/// <para>
	/// These are dice, not flat numbers: Bless adds 1d4, so the bonus is rolled fresh each time and
	/// cannot be folded into a mobile's stats. That is why this is consulted at the roll rather than
	/// baked into an attack bonus.
	/// </para>
	/// <para>
	/// In Server/ for the same reason conditions are: <see cref="CombatRules"/> has to consult it on
	/// every saving throw, and Server cannot see the Scripts layer.
	/// </para>
	/// </summary>
	public static class DnDRollModifiers
	{
		private sealed class Entry
		{
			public string Source;
			public int DiceSides;

			/// <summary>+1 for a bonus, -1 for a penalty.</summary>
			public int Sign;

			public RollKind Kinds;
			public DateTime Expires;

			/// <summary>Guidance and Resistance are spent by the first roll that uses them.</summary>
			public bool OneShot;
		}

		private static readonly Dictionary<Mobile, List<Entry>> m_Table = new Dictionary<Mobile, List<Entry>>();

		public static void Add(
			Mobile m, string source, int diceSides, int sign, RollKind kinds, TimeSpan duration, bool oneShot)
		{
			if (m == null || diceSides <= 0 || kinds == RollKind.None)
			{
				return;
			}

			List<Entry> entries;

			if (!m_Table.TryGetValue(m, out entries))
			{
				m_Table[m] = entries = new List<Entry>();
			}

			// A second casting of the same spell replaces the first rather than stacking.
			entries.RemoveAll(e => e.Source == source);

			entries.Add(
				new Entry
				{
					Source = source,
					DiceSides = diceSides,
					Sign = sign >= 0 ? 1 : -1,
					Kinds = kinds,
					Expires = duration == TimeSpan.Zero ? DateTime.MaxValue : DateTime.UtcNow + duration,
					OneShot = oneShot
				});
		}

		public static void Remove(Mobile m, string source)
		{
			List<Entry> entries;

			if (m == null || !m_Table.TryGetValue(m, out entries))
			{
				return;
			}

			entries.RemoveAll(e => e.Source == source);

			if (entries.Count == 0)
			{
				m_Table.Remove(m);
			}
		}

		public static void Clear(Mobile m)
		{
			if (m != null)
			{
				m_Table.Remove(m);
			}
		}

		/// <summary>
		/// Rolls every modifier that applies to this kind of roll and returns their total. One-shot
		/// modifiers are consumed here, which is why this must be called once per roll and its
		/// result reused rather than called again for the same roll.
		/// </summary>
		public static int Roll(Mobile m, RollKind kind)
		{
			List<Entry> entries;

			if (m == null || kind == RollKind.None || !m_Table.TryGetValue(m, out entries))
			{
				return 0;
			}

			DateTime now = DateTime.UtcNow;
			int total = 0;

			for (int i = entries.Count - 1; i >= 0; --i)
			{
				Entry entry = entries[i];

				if (now >= entry.Expires)
				{
					entries.RemoveAt(i);
					continue;
				}

				if ((entry.Kinds & kind) == 0)
				{
					continue;
				}

				total += entry.Sign * Utility.RandomMinMax(1, entry.DiceSides);

				if (entry.OneShot)
				{
					entries.RemoveAt(i);
				}
			}

			if (entries.Count == 0)
			{
				m_Table.Remove(m);
			}

			return total;
		}

		/// <summary>Whether anything is currently modifying this kind of roll, without spending it.</summary>
		public static bool Has(Mobile m, RollKind kind)
		{
			List<Entry> entries;

			if (m == null || !m_Table.TryGetValue(m, out entries))
			{
				return false;
			}

			DateTime now = DateTime.UtcNow;

			foreach (Entry entry in entries)
			{
				if (now < entry.Expires && (entry.Kinds & kind) != 0)
				{
					return true;
				}
			}

			return false;
		}
	}
}
