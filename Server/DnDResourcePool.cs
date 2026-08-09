using System;
using System.Collections.Generic;

namespace Server
{
	/// <summary>Which pool a class spends from. One per mechanism, not one per class.</summary>
	public enum ResourcePoolType
	{
		/// <summary>Monk ki: one point per Monk level, back on a short rest.</summary>
		Ki,

		/// <summary>Sorcery points: one per Sorcerer level from 2nd, back on a long rest.</summary>
		Sorcery,

		/// <summary>Lay on Hands: a pool of hit points, five per Paladin level, long rest.</summary>
		LayOnHands
	}

	/// <summary>
	/// Pools of points a character spends several of at once.
	/// <para>
	/// This is the distinction the activated-feature system could not express, and the reason those
	/// classes were blocked: <c>FeatureUses</c> counts uses, and a use is all-or-nothing. A Monk
	/// spending 2 ki of 5 on Flurry of Blows and 3 on Stunning Strike is not doing the same thing
	/// as spending one of three uses, and modelling it that way would have made every ki ability
	/// cost the same regardless of what it was.
	/// </para>
	/// <para>
	/// Kept in Server beside the other rules the engine consults, and keyed by mobile rather than
	/// stored on DnDPlayerMobile, for the same reason conditions are: the rules that spend from a
	/// pool live here, and a side table means the character class does not have to know about every
	/// pool that might ever exist.
	/// </para>
	/// </summary>
	public static class DnDResourcePools
	{
		private sealed class Pool
		{
			public int Spent;
		}

		private static readonly Dictionary<Mobile, Dictionary<ResourcePoolType, Pool>> m_Pools =
			new Dictionary<Mobile, Dictionary<ResourcePoolType, Pool>>();

		/// <summary>
		/// The size of a pool, which is a function of class level rather than a stored number - so
		/// levelling up widens it without anything having to remember to.
		/// </summary>
		public static int GetMaximum(IDnDCharacter character, ResourcePoolType type)
		{
			if (character == null || character.Classes == null)
			{
				return 0;
			}

			int level = GetClassLevel(character, ClassFor(type));

			switch (type)
			{
				case ResourcePoolType.Ki:
					// Ki arrives at 2nd level, then one point per Monk level.
					return level >= 2 ? level : 0;

				case ResourcePoolType.Sorcery:
					// Sorcery points arrive at 2nd level and equal the Sorcerer level.
					return level >= 2 ? level : 0;

				case ResourcePoolType.LayOnHands:
					// Five hit points per Paladin level, spent in any amounts you like. This is the
					// version the rules actually describe - it was a flat 5 per use before, which
					// made a 10th level Paladin no better at it than a 1st level one.
					return level * 5;
			}

			return 0;
		}

		private static string ClassFor(ResourcePoolType type)
		{
			switch (type)
			{
				case ResourcePoolType.Ki: return "Monk";
				case ResourcePoolType.Sorcery: return "Sorcerer";
				case ResourcePoolType.LayOnHands: return "Paladin";
			}

			return null;
		}

		/// <summary>
		/// Levels in one class, counting a subclass towards its parent - a Way of the Open Hand
		/// monk is a Monk, and their ki should not vanish the moment they specialise.
		/// </summary>
		private static int GetClassLevel(IDnDCharacter character, string className)
		{
			if (className == null)
			{
				return 0;
			}

			int total = 0;

			foreach (var kv in character.Classes)
			{
				CharacterClass c = kv.Key;

				while (c != null)
				{
					if (Insensitive.Equals(c.Name, className))
					{
						total += kv.Value;
						break;
					}

					c = c.GetParent();
				}
			}

			return total;
		}

		public static int GetRemaining(Mobile m, IDnDCharacter character, ResourcePoolType type)
		{
			return Math.Max(0, GetMaximum(character, type) - GetSpent(m, type));
		}

		private static int GetSpent(Mobile m, ResourcePoolType type)
		{
			Dictionary<ResourcePoolType, Pool> pools;
			Pool pool;

			if (m == null || !m_Pools.TryGetValue(m, out pools) || !pools.TryGetValue(type, out pool))
			{
				return 0;
			}

			return pool.Spent;
		}

		/// <summary>
		/// Spends points, all or nothing. Partial spends are refused rather than clamped, because
		/// a Monk who meant to spend 3 ki and had 2 should not get a weaker Flurry of Blows - they
		/// should be told they cannot afford it.
		/// </summary>
		public static bool Spend(Mobile m, IDnDCharacter character, ResourcePoolType type, int amount)
		{
			if (m == null || amount <= 0 || GetRemaining(m, character, type) < amount)
			{
				return false;
			}

			Dictionary<ResourcePoolType, Pool> pools;

			if (!m_Pools.TryGetValue(m, out pools))
			{
				pools = new Dictionary<ResourcePoolType, Pool>();
				m_Pools[m] = pools;
			}

			Pool pool;

			if (!pools.TryGetValue(type, out pool))
			{
				pool = new Pool();
				pools[type] = pool;
			}

			pool.Spent += amount;

			return true;
		}

		/// <summary>
		/// Refills what a rest refills. Ki comes back on a short rest; the others need a long one,
		/// which is the whole difference in feel between a Monk and a Sorcerer over a day.
		/// </summary>
		public static void Restore(Mobile m, bool longRest)
		{
			Dictionary<ResourcePoolType, Pool> pools;

			if (m == null || !m_Pools.TryGetValue(m, out pools))
			{
				return;
			}

			var restored = new List<ResourcePoolType>();

			foreach (var kv in pools)
			{
				if (longRest || RecoversOnShortRest(kv.Key))
				{
					restored.Add(kv.Key);
				}
			}

			foreach (ResourcePoolType type in restored)
			{
				pools[type].Spent = 0;
			}
		}

		public static bool RecoversOnShortRest(ResourcePoolType type)
		{
			return type == ResourcePoolType.Ki;
		}

		public static void Clear(Mobile m)
		{
			if (m != null)
			{
				m_Pools.Remove(m);
			}
		}
	}
}
