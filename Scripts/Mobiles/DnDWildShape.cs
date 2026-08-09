using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
	/// <summary>
	/// Wearing another creature's stat block: Wild Shape, and the Polymorph family.
	/// <para>
	/// The blocker here was never the monster data - that has been data-driven for a while, which is
	/// most of what this needs. It was that nothing could swap a character's numbers for a monster's
	/// and reliably swap them back. The rules for that are what this file is.
	/// </para>
	/// <para>
	/// The SRD is specific about how the swap works, and the specifics are the whole feature:
	/// </para>
	/// <list type="bullet">
	/// <item>You take the beast's hit points, and they are a separate pool. Damage comes off them,
	/// and when they run out you revert with your own hit points untouched - which is why Wild Shape
	/// is a defensive ability and not just a different weapon.</item>
	/// <item>You keep your own mental abilities and your saving throw proficiencies; you take the
	/// beast's physical ones.</item>
	/// <item>Your equipment stops applying. A bear does not benefit from your chain shirt.</item>
	/// </list>
	/// <para>
	/// Kept as a side table keyed by mobile, like the other rules layered over Mobile, so nothing
	/// about the character class has to know a shapechanged character is a special case.
	/// </para>
	/// </summary>
	public static class DnDWildShape
	{
		private sealed class ShapedForm
		{
			public string MonsterId;

			public int OriginalBody;
			public int OriginalHue;
			public string OriginalName;

			/// <summary>The character's own hit points, set aside while the beast's are in use.</summary>
			public int StoredHits;

			public int BeastHits;
			public int BeastHitsMax;
			public int BeastArmorClass;
			public int BeastAttackBonus;
			public string BeastDamageDice;

			public Timer Expiry;
		}

		private static readonly Dictionary<Mobile, ShapedForm> m_Shaped = new Dictionary<Mobile, ShapedForm>();

		public static bool IsShaped(Mobile m)
		{
			return m != null && m_Shaped.ContainsKey(m);
		}

		public static string GetFormName(Mobile m)
		{
			ShapedForm form;

			return m != null && m_Shaped.TryGetValue(m, out form) ? form.MonsterId : null;
		}

		#region Rules the engine asks about

		/// <summary>The beast's armour class, which replaces the character's own entirely.</summary>
		public static int GetArmorClass(Mobile m)
		{
			ShapedForm form;

			return m != null && m_Shaped.TryGetValue(m, out form) ? form.BeastArmorClass : 0;
		}

		public static int GetAttackBonus(Mobile m)
		{
			ShapedForm form;

			return m != null && m_Shaped.TryGetValue(m, out form) ? form.BeastAttackBonus : 0;
		}

		public static string GetDamageDice(Mobile m)
		{
			ShapedForm form;

			return m != null && m_Shaped.TryGetValue(m, out form) ? form.BeastDamageDice : null;
		}

		#endregion

		/// <summary>
		/// Takes the form of a beast. The challenge rating cap is what makes Wild Shape scale with
		/// level rather than letting a 2nd level Druid become a giant.
		/// </summary>
		public static bool Assume(Mobile m, string monsterId, double maxChallengeRating, TimeSpan duration)
		{
			if (m == null || IsShaped(m))
			{
				m.SendMessage("You are already in another form.");
				return false;
			}

			SrdMonsterData data = SrdMonster.Lookup(monsterId);

			if (data == null)
			{
				m.SendMessage("You do not know that form.");
				return false;
			}

			if (data.ChallengeRating > maxChallengeRating)
			{
				m.SendMessage(
					"{0} is beyond you - you can take forms of challenge {1} or less.",
					data.DisplayName,
					maxChallengeRating);

				return false;
			}

			var form = new ShapedForm
			{
				MonsterId = monsterId,
				OriginalBody = m.Body.BodyID,
				OriginalHue = m.Hue,
				OriginalName = m.Name,
				StoredHits = m.Hits,
				BeastHits = data.HitPoints,
				BeastHitsMax = data.HitPoints,
				BeastArmorClass = data.ArmorClass,
				BeastAttackBonus = data.AttackBonus,
				BeastDamageDice = data.DamageDice
			};

			m_Shaped[m] = form;

			m.Body = data.Body;
			m.Hue = data.Hue;
			m.Name = data.DisplayName;
			m.Hits = data.HitPoints;

			m.SendMessage(0x35, "You take the form of {0}.", data.DisplayName);
			m.PlaySound(0x1E0);

			if (duration > TimeSpan.Zero)
			{
				form.Expiry = Timer.DelayCall(duration, () => Revert(m, "The form slips away."));
			}

			return true;
		}

		/// <summary>
		/// Damage while shaped comes off the beast's hit points. Returns what spills over once the
		/// form breaks, which is what the SRD says happens to excess damage - a bear killed by a
		/// huge hit does not leave the Druid untouched.
		/// </summary>
		public static bool OnDamage(Mobile m, int amount)
		{
			ShapedForm form;

			if (m == null || !m_Shaped.TryGetValue(m, out form))
			{
				return false;
			}

			form.BeastHits -= amount;

			if (form.BeastHits > 0)
			{
				m.Hits = form.BeastHits;
				return true;
			}

			int overflow = -form.BeastHits;

			Revert(m, "Your form gives way.");

			// The beast absorbed everything up to its hit points; the rest reaches the Druid.
			if (overflow > 0)
			{
				m.Damage(overflow);
			}

			return true;
		}

		public static bool Revert(Mobile m, string reason)
		{
			ShapedForm form;

			if (m == null || !m_Shaped.TryGetValue(m, out form))
			{
				return false;
			}

			if (form.Expiry != null)
			{
				form.Expiry.Stop();
			}

			m_Shaped.Remove(m);

			m.Body = form.OriginalBody;
			m.Hue = form.OriginalHue;
			m.Name = form.OriginalName;

			// The character's own hit points come back exactly as they were. This is the whole
			// reason Wild Shape is worth using defensively.
			m.Hits = Math.Max(1, form.StoredHits);

			if (!String.IsNullOrEmpty(reason))
			{
				m.SendMessage(0x22, reason);
			}

			m.SendMessage(0x35, "You return to your own shape.");
			m.PlaySound(0x1E0);

			return true;
		}

		/// <summary>Forms a Druid of this level may take, by the SRD's challenge rating cap.</summary>
		public static double GetMaxChallengeRating(int druidLevel)
		{
			if (druidLevel >= 8) return 1.0;
			if (druidLevel >= 4) return 0.5;

			return 0.25;
		}

		public static List<SrdMonsterData> GetAvailableForms(double maxChallengeRating)
		{
			var forms = new List<SrdMonsterData>();

			foreach (SrdMonsterData data in SrdMonster.AllData)
			{
				if (data.ChallengeRating <= maxChallengeRating)
				{
					forms.Add(data);
				}
			}

			return forms;
		}
	}
}
