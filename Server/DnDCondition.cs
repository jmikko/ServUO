#region References
using System;
using System.Collections.Generic;
#endregion

namespace Server
{
	/// <summary>
	/// The SRD conditions. Flags because they stack freely - a creature can be poisoned, prone and
	/// frightened at once, and each contributes its own effects.
	/// </summary>
	[Flags]
	public enum DnDCondition
	{
		None = 0x0000,
		Blinded = 0x0001,
		Charmed = 0x0002,
		Deafened = 0x0004,
		Frightened = 0x0008,
		Grappled = 0x0010,
		Incapacitated = 0x0020,
		Invisible = 0x0040,
		Paralyzed = 0x0080,
		Petrified = 0x0100,
		Poisoned = 0x0200,
		Prone = 0x0400,
		Restrained = 0x0800,
		Stunned = 0x1000,
		Unconscious = 0x2000,

		/// <summary>Conditions that stop a creature acting at all.</summary>
		CannotAct = Incapacitated | Paralyzed | Petrified | Stunned | Unconscious,

		/// <summary>Conditions that give the sufferer disadvantage on its own attack rolls.</summary>
		AttackDisadvantage = Blinded | Frightened | Poisoned | Prone | Restrained,

		/// <summary>Conditions that give attackers advantage against the sufferer.</summary>
		DefenceAdvantage = Blinded | Paralyzed | Petrified | Restrained | Stunned | Unconscious,

		/// <summary>Conditions that auto-fail Strength and Dexterity saving throws.</summary>
		AutoFailStrDexSaves = Paralyzed | Petrified | Stunned | Unconscious
	}

	/// <summary>
	/// Which conditions each mobile currently suffers, and when they lapse.
	/// <para>
	/// Lives in Server/ rather than alongside the spells that inflict them because
	/// <see cref="CombatRules"/> has to consult it on every attack roll and saving throw, and
	/// Server cannot see the Scripts layer. Kept as a side table keyed by mobile so that creatures
	/// and players - which share no base beyond Mobile - both get conditions for free.
	/// </para>
	/// </summary>
	public static class DnDConditions
	{
		private sealed class Entry
		{
			public DnDCondition Condition;
			public DateTime Expires;
		}

		private static readonly Dictionary<Mobile, List<Entry>> m_Table = new Dictionary<Mobile, List<Entry>>();

		/// <summary>Inflicts a condition for a while. TimeSpan.Zero means until removed.</summary>
		public static void Add(Mobile m, DnDCondition condition, TimeSpan duration)
		{
			if (m == null || condition == DnDCondition.None)
			{
				return;
			}

			List<Entry> entries;

			if (!m_Table.TryGetValue(m, out entries))
			{
				m_Table[m] = entries = new List<Entry>();
			}

			entries.Add(
				new Entry
				{
					Condition = condition,
					Expires = duration == TimeSpan.Zero ? DateTime.MaxValue : DateTime.UtcNow + duration
				});
		}

		public static void Remove(Mobile m, DnDCondition condition)
		{
			List<Entry> entries;

			if (m == null || !m_Table.TryGetValue(m, out entries))
			{
				return;
			}

			entries.RemoveAll(e => (e.Condition & condition) != 0);

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

		/// <summary>Everything currently afflicting this mobile, with lapsed entries dropped.</summary>
		public static DnDCondition Get(Mobile m)
		{
			List<Entry> entries;

			if (m == null || !m_Table.TryGetValue(m, out entries))
			{
				return DnDCondition.None;
			}

			DateTime now = DateTime.UtcNow;
			DnDCondition result = DnDCondition.None;

			for (int i = entries.Count - 1; i >= 0; --i)
			{
				if (now >= entries[i].Expires)
				{
					entries.RemoveAt(i);
					continue;
				}

				result |= entries[i].Condition;
			}

			if (entries.Count == 0)
			{
				m_Table.Remove(m);
			}

			return result;
		}

		public static bool Has(Mobile m, DnDCondition condition)
		{
			return (Get(m) & condition) != 0;
		}

		/// <summary>Whether this mobile is able to take actions at all.</summary>
		public static bool CanAct(Mobile m)
		{
			return (Get(m) & DnDCondition.CannotAct) == 0;
		}

		/// <summary>
		/// How <paramref name="attacker"/> rolls to hit <paramref name="defender"/>, combining the
		/// attacker's own impairments with the defender's vulnerabilities. Advantage and
		/// disadvantage cancel rather than stacking, so one of each is a normal roll.
		/// </summary>
		public static RollMode GetAttackRollMode(Mobile attacker, IDamageable defender)
		{
			bool disadvantage = (Get(attacker) & DnDCondition.AttackDisadvantage) != 0;

			var target = defender as Mobile;
			bool advantage = target != null && (Get(target) & DnDCondition.DefenceAdvantage) != 0;

			if (advantage == disadvantage)
			{
				return RollMode.Normal;
			}

			return advantage ? RollMode.Advantage : RollMode.Disadvantage;
		}

		/// <summary>A paralysed or unconscious creature simply fails Strength and Dexterity saves.</summary>
		public static bool AutoFailsSave(Mobile m, AbilityScoreType ability)
		{
			if (ability != AbilityScoreType.Str && ability != AbilityScoreType.Dex)
			{
				return false;
			}

			return (Get(m) & DnDCondition.AutoFailStrDexSaves) != 0;
		}
	}
}
