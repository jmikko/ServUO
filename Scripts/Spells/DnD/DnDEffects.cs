using System;
using System.Collections.Generic;

namespace Server.Spells.DnD
{
	/// <summary>
	/// Timed magical effects on a mobile.
	/// <para>
	/// Kept as a side table keyed by mobile rather than as fields on DnDPlayerMobile, because
	/// effects apply to creatures too and neither of those types shares a base beyond Mobile. It
	/// also means an effect expiring is a timer here rather than something every mobile has to
	/// remember to tick.
	/// </para>
	/// </summary>
	public static class DnDEffects
	{
		private sealed class ArmorClassEffect
		{
			public int Value;
			public string Source;
			public DateTime Expires;
		}

		private static readonly Dictionary<Mobile, ArmorClassEffect> m_ArmorClass =
			new Dictionary<Mobile, ArmorClassEffect>();
			
		private static readonly Dictionary<Mobile, List<ArmorClassEffect>> m_ArmorClassBonus =
			new Dictionary<Mobile, List<ArmorClassEffect>>();

		private sealed class MovementModeEffect
		{
			public string Source;
			public DateTime Expires;
		}

		private static readonly Dictionary<Mobile, MovementModeEffect> m_Flying =
			new Dictionary<Mobile, MovementModeEffect>();

		/// <summary>
		/// Sets a floor on the target's armour class for a while - Mage Armor and its relatives
		/// replace a low unarmoured AC rather than adding to whatever is already there.
		/// </summary>
		public static void ApplyArmorClass(Mobile target, int value, TimeSpan duration, string source)
		{
			if (target == null || value <= 0)
			{
				return;
			}

			m_ArmorClass[target] = new ArmorClassEffect
			{
				Value = value,
				Source = source,
				Expires = DateTime.UtcNow + duration
			};

			target.SendMessage("{0} surrounds you.", source);
			target.Delta(MobileDelta.Attributes);
		}

		/// <summary>The armour class this mobile's magic guarantees, or 0 for none.</summary>
		public static int GetArmorClassFloor(Mobile target)
		{
			ArmorClassEffect effect;

			if (target == null || !m_ArmorClass.TryGetValue(target, out effect))
			{
				return 0;
			}

			if (DateTime.UtcNow >= effect.Expires)
			{
				m_ArmorClass.Remove(target);

				target.SendMessage("{0} fades.", effect.Source);

				return 0;
			}

			return effect.Value;
		}

		public static void ApplyArmorClassBonus(Mobile target, int bonus, TimeSpan duration, string source)
		{
			if (target == null || bonus == 0) return;

			List<ArmorClassEffect> list;
			if (!m_ArmorClassBonus.TryGetValue(target, out list))
			{
				m_ArmorClassBonus[target] = list = new List<ArmorClassEffect>();
			}

			list.Add(new ArmorClassEffect
			{
				Value = bonus,
				Source = source,
				Expires = DateTime.UtcNow + duration
			});
			
			target.Delta(MobileDelta.Attributes);
		}

		public static int GetArmorClassBonus(Mobile target)
		{
			List<ArmorClassEffect> list;
			if (target == null || !m_ArmorClassBonus.TryGetValue(target, out list))
			{
				return 0;
			}

			int bonus = 0;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				ArmorClassEffect effect = list[i];
				if (DateTime.UtcNow >= effect.Expires)
				{
					list.RemoveAt(i);
				}
				else
				{
					bonus += effect.Value;
				}
			}

			if (list.Count == 0)
			{
				m_ArmorClassBonus.Remove(target);
			}

			return bonus;
		}

		// Advantage and damage resistance used to live here. They moved to Server/DnDRollModifier.cs,
		// because CombatRules consults them on every save and every swing and the engine cannot see
		// Scripts - the same reason conditions and roll modifiers live there. Light stays, since only
		// DnDPlayerMobile reads it.

		private sealed class TimedEffect
		{
			public string Source;
			public DateTime Expires;
		}

		private static readonly Dictionary<Mobile, TimedEffect> m_Light =
			new Dictionary<Mobile, TimedEffect>();

		/// <summary>
		/// Magical light. Read by ComputeBaseLightLevels the same way species darkvision is, so a
		/// Light spell and a Dwarf.s eyes reach the player through one path rather than two.
		/// </summary>
		public static void ApplyLight(Mobile target, TimeSpan duration, string source)
		{
			if (target == null)
			{
				return;
			}

			m_Light[target] = new TimedEffect
			{
				Source = source,
				Expires = DateTime.UtcNow + duration
			};

			target.SendMessage("{0} illuminates your surroundings.", source);
			target.Delta(MobileDelta.Attributes);
		}

		public static bool HasLight(Mobile target)
		{
			TimedEffect effect;

			if (target == null || !m_Light.TryGetValue(target, out effect))
			{
				return false;
			}

			if (DateTime.UtcNow >= effect.Expires)
			{
				m_Light.Remove(target);
				target.SendMessage("{0} fades.", effect.Source);

				return false;
			}

			return true;
		}

		/// <summary>
		/// Ends every magical effect on the target - Dispel Magic and Counterspell, which are the
		/// only reason this needed to be reachable from a spell rather than only from a long rest.
		/// </summary>
		public static void Clear(Mobile target)
		{
			m_ArmorClass.Remove(target);
			DnDRollModifiers.ClearAdvantage(target);
			m_Light.Remove(target);


			if (m_Flying.Remove(target))
			{
				target.Flying = false;
			}
		}

		public static void ApplyFlying(Mobile target, TimeSpan duration, string source)
		{
			if (target == null)
			{
				return;
			}

			if (m_Flying.ContainsKey(target))
			{
				m_Flying[target].Expires = DateTime.UtcNow + duration;
				return;
			}

			m_Flying[target] = new MovementModeEffect
			{
				Source = source,
				Expires = DateTime.UtcNow + duration
			};

			target.Flying = true;
			target.SendMessage("You gain a flying speed from {0}.", source);

			Timer.DelayCall(duration, () =>
			{
				if (m_Flying.ContainsKey(target))
				{
					var effect = m_Flying[target];
					if (DateTime.UtcNow >= effect.Expires)
					{
						m_Flying.Remove(target);
						target.Flying = false;
						target.SendMessage("{0} fades and you lose your flying speed.", effect.Source);
					}
					else
					{
						// Reschedule if it was extended
						Timer.DelayCall(effect.Expires - DateTime.UtcNow, () => CheckFlying(target));
					}
				}
			});
		}

		private static void CheckFlying(Mobile target)
		{
			if (target != null && m_Flying.ContainsKey(target))
			{
				var effect = m_Flying[target];
				if (DateTime.UtcNow >= effect.Expires)
				{
					m_Flying.Remove(target);
					target.Flying = false;
					target.SendMessage("{0} fades and you lose your flying speed.", effect.Source);
				}
			}
		}
		#region Hunter's Mark
		
		private sealed class HuntersMarkEffect
		{
			public Mobile MarkedTarget;
			public DateTime Expires;
		}
		
		private static readonly Dictionary<Mobile, HuntersMarkEffect> m_HuntersMark = new Dictionary<Mobile, HuntersMarkEffect>();
		
		public static void ApplyHuntersMark(Mobile caster, Mobile target, TimeSpan duration)
		{
			if (caster == null || target == null)
			{
				return;
			}
			
			m_HuntersMark[caster] = new HuntersMarkEffect
			{
				MarkedTarget = target,
				Expires = DateTime.UtcNow + duration
			};
		}
		
		public static bool HasHuntersMark(Mobile caster, Mobile target)
		{
			HuntersMarkEffect effect;
			if (caster == null || target == null || !m_HuntersMark.TryGetValue(caster, out effect))
			{
				return false;
			}
			
			if (DateTime.UtcNow >= effect.Expires)
			{
				m_HuntersMark.Remove(caster);
				return false;
			}
			
			return effect.MarkedTarget == target;
		}
		
		#endregion

	}
}
