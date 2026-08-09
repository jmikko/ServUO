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
		AbilityCheck = 0x4,
		Damage = 0x8
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
			
			/// <summary>Flat bonus added to the roll.</summary>
			public int Bonus;

			/// <summary>+1 for a bonus, -1 for a penalty.</summary>
			public int Sign;

			public RollKind Kinds;
			public DateTime Expires;

			/// <summary>Guidance and Resistance are spent by the first roll that uses them.</summary>
			public bool OneShot;
		}

		private static readonly Dictionary<Mobile, List<Entry>> m_Table = new Dictionary<Mobile, List<Entry>>();

		public static void Add(
			Mobile m, string source, int diceSides, int bonus, int sign, RollKind kinds, TimeSpan duration, bool oneShot)
		{
			if (m == null || (diceSides <= 0 && bonus == 0) || kinds == RollKind.None)
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
					Bonus = bonus,
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

				if (entry.DiceSides > 0)
				{
					total += entry.Sign * Utility.RandomMinMax(1, entry.DiceSides);
				}
				
				total += entry.Sign * entry.Bonus;

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

		#region Advantage and resistance

		// These sit beside the roll modifiers rather than with the other spell effects in Scripts,
		// because CombatRules consults them on every save and DnDCombat on every swing, and the
		// engine cannot see Scripts. Same reason conditions live in Server.

		private sealed class TimedEffect
		{
			public string Source;
			public DateTime Expires;
			public RollKind Kinds;
		}

		private static readonly Dictionary<Mobile, TimedEffect> m_Advantage =
			new Dictionary<Mobile, TimedEffect>();

		private static readonly Dictionary<Mobile, TimedEffect> m_Resistance =
			new Dictionary<Mobile, TimedEffect>();

		/// <summary>
		/// Grants advantage on a kind of roll for a while - True Strike, Enhance Ability, Beacon
		/// of Hope.
		/// <para>
		/// Deliberately not a roll modifier, even though it lives in the same file: advantage is
		/// rolling twice and keeping the better, worth about five points in the middle of the range
		/// and nothing at either end. A flat die would be a different rule wearing its name.
		/// </para>
		/// </summary>
		public static void AddAdvantage(Mobile m, RollKind kinds, TimeSpan duration, string source)
		{
			if (m == null || kinds == RollKind.None)
			{
				return;
			}

			m_Advantage[m] = new TimedEffect
			{
				Source = source,
				Kinds = kinds,
				Expires = DateTime.UtcNow + duration
			};
		}

		public static bool HasAdvantage(Mobile m, RollKind kind)
		{
			return IsActive(m_Advantage, m, kind);
		}

		/// <summary>Halves incoming physical damage - Blade Ward and the protection family.</summary>
		public static void AddResistance(Mobile m, TimeSpan duration, string source)
		{
			if (m == null)
			{
				return;
			}

			m_Resistance[m] = new TimedEffect
			{
				Source = source,
				Kinds = RollKind.Attack,
				Expires = DateTime.UtcNow + duration
			};
		}

		public static bool HasResistance(Mobile m)
		{
			return IsActive(m_Resistance, m, RollKind.Attack);
		}

		public static void ClearAdvantage(Mobile m)
		{
			m_Advantage.Remove(m);
			m_Resistance.Remove(m);
		}

		private static bool IsActive(Dictionary<Mobile, TimedEffect> table, Mobile m, RollKind kind)
		{
			TimedEffect effect;

			if (m == null || !table.TryGetValue(m, out effect))
			{
				return false;
			}

			if (DateTime.UtcNow >= effect.Expires)
			{
				table.Remove(m);

				m.SendMessage("{0} fades.", effect.Source);

				return false;
			}

			return (effect.Kinds & kind) != 0;
		}

		#endregion
	}
}
