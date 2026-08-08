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

		public static void Clear(Mobile target)
		{
			m_ArmorClass.Remove(target);
		}
	}
}
