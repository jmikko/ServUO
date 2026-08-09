using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
	/// <summary>
	/// Dying, the D&amp;D way: at 0 hit points you fall unconscious rather than die, and then roll
	/// to see whether you recover or slip away.
	/// <para>
	/// This replaces Ultima Online's death outright. In UO you become a ghost the instant you run
	/// out of hit points and go looking for a healer; in D&amp;D 0 hit points is the beginning of a
	/// tense thirty seconds in which your friends can still save you. It is one of the loudest
	/// differences between the two games, and the one a player notices first.
	/// </para>
	/// <para>
	/// Each round: a d20 against 10. Three successes and you stabilise, three failures and you die.
	/// A natural 20 puts you back on your feet with a single hit point; a natural 1 counts twice.
	/// Being hit while down is a failure on its own, and a critical hit is two.
	/// </para>
	/// </summary>
	public static class DnDDeath
	{
		/// <summary>An SRD round. Death saves are rolled once per round, so this is the pace.</summary>
		private static readonly TimeSpan RoundLength = TimeSpan.FromSeconds(6.0);

		private const int DeathSaveDC = 10;
		private const int Needed = 3;

		/// <summary>What one death save did. Named so the test can assert on it directly.</summary>
		public enum SaveOutcome
		{
			Continues,
			Stabilised,
			Revived,
			Died
		}

		public sealed class DyingState
		{
			public int Successes;
			public int Failures;
			public Timer Timer;
			public bool Stable;
		}

		/// <summary>
		/// One death save, as arithmetic on a counter pair and nothing else.
		/// <para>
		/// This is split out from the timer deliberately. The interesting part of dying is a
		/// distribution - roughly two in five characters left alone at 0 hit points do not get up
		/// again - and a distribution can only be measured by running the rule thousands of times,
		/// which a six-second timer will not do. So the rule lives here, the timer calls it, and the
		/// self-test calls the same function.
		/// </para>
		/// </summary>
		public static SaveOutcome ApplyRoll(DyingState state, int roll)
		{
			if (state == null)
			{
				return SaveOutcome.Continues;
			}

			if (roll >= 20)
			{
				// A natural 20 is not merely a success: you come round with one hit point.
				return SaveOutcome.Revived;
			}

			if (roll <= 1)
			{
				// A natural 1 counts as two failures, which is what makes a single bad roll able
				// to finish a character who was otherwise doing fine.
				state.Failures += 2;
			}
			else if (roll >= DeathSaveDC)
			{
				++state.Successes;
			}
			else
			{
				++state.Failures;
			}

			if (state.Failures >= Needed)
			{
				return SaveOutcome.Died;
			}

			if (state.Successes >= Needed)
			{
				state.Stable = true;
				return SaveOutcome.Stabilised;
			}

			return SaveOutcome.Continues;
		}

		private static readonly Dictionary<Mobile, DyingState> m_Dying = new Dictionary<Mobile, DyingState>();

		public static bool IsDying(Mobile m)
		{
			return m != null && m_Dying.ContainsKey(m);
		}

		public static bool IsStable(Mobile m)
		{
			DyingState state;

			return m != null && m_Dying.TryGetValue(m, out state) && state.Stable;
		}

		/// <summary>
		/// Called from OnBeforeDeath. Returns false to refuse the death and start dying instead.
		/// <para>
		/// Only players get this. A monster at 0 hit points is simply dead - giving every goblin
		/// three rounds of death saves would make every fight drag and is not what the rules say.
		/// </para>
		/// </summary>
		public static bool OnBeforeDeath(Mobile m)
		{
			// Gated on the type, not on Mobile.Player. Player is set when a client attaches, so a
			// character with nobody logged into it - a test character, or one mid-reconnect - would
			// skip death saves entirely and just die. Being a player character is a property of the
			// character, not of whether someone is currently watching.
			if (!(m is DnDPlayerMobile))
			{
				return true;
			}

			DyingState state;

			// Already dying and out of saves - let the death through this time.
			if (m_Dying.TryGetValue(m, out state) && state.Failures >= Needed)
			{
				Stop(m);
				return true;
			}

			BeginDying(m);

			return false;
		}

		private static void BeginDying(Mobile m)
		{
			if (m_Dying.ContainsKey(m))
			{
				return;
			}

			var state = new DyingState();

			m_Dying[m] = state;

			m.Hits = 0;
			m.Frozen = true;

			DnDConditions.Add(m, DnDCondition.Unconscious, TimeSpan.Zero);

			m.SendMessage(0x22, "You are dying. Three successes and you stabilise; three failures and you do not.");
			m.PublicOverheadMessage(Network.MessageType.Regular, 0x22, false, m.Name + " collapses.");

			state.Timer = Timer.DelayCall(RoundLength, RoundLength, () => RollDeathSave(m));
		}

		private static void RollDeathSave(Mobile m)
		{
			DyingState state;

			if (m == null || m.Deleted || !m_Dying.TryGetValue(m, out state))
			{
				Stop(m);
				return;
			}

			// Healed back up by someone else - nothing left to roll for.
			if (m.Hits > 0)
			{
				Revive(m, false);
				return;
			}

			// Logging out while dying must not kill you in your absence - there would be nothing
			// you or anyone else could do about it. The saves wait until you are back.
			if (m.Map == Map.Internal || m.NetState == null)
			{
				return;
			}

			if (state.Stable)
			{
				return;
			}

			int roll = Utility.RandomMinMax(1, 20);

			switch (ApplyRoll(state, roll))
			{
				case SaveOutcome.Revived:
					{
						m.Hits = 1;

						Revive(m, false);
						m.SendMessage(0x40, "You come to your senses.");

						break;
					}
				case SaveOutcome.Stabilised:
					{
						m.SendMessage(0x40, "You are stable, but still unconscious.");
						break;
					}
				case SaveOutcome.Died:
					{
						Die(m);
						break;
					}
				default:
					{
						if (roll >= DeathSaveDC)
						{
							m.SendMessage(0x40, "You cling on. ({0} successes)", state.Successes);
						}
						else
						{
							m.SendMessage(0x22, "You weaken. ({0} failures)", state.Failures);
						}

						break;
					}
			}
		}

		/// <summary>
		/// Damage taken while down is a failed save on its own, and a critical is two - which is
		/// what makes finishing someone off possible, and what makes standing over a fallen ally
		/// worth doing.
		/// </summary>
		public static void OnDamagedWhileDying(Mobile m, bool critical)
		{
			DyingState state;

			if (m == null || !m_Dying.TryGetValue(m, out state))
			{
				return;
			}

			state.Stable = false;
			state.Failures += critical ? 2 : 1;

			m.SendMessage(0x22, "Being struck while down costs you. ({0} failures)", Math.Min(Needed, state.Failures));

			if (state.Failures >= Needed)
			{
				Die(m);
			}
		}

		/// <summary>Any healing at all brings a dying character back to consciousness.</summary>
		public static void OnHealed(Mobile m)
		{
			if (IsDying(m) && m.Hits > 0)
			{
				Revive(m, true);
			}
		}

		private static void Revive(Mobile m, bool announce)
		{
			Stop(m);

			m.Frozen = false;

			DnDConditions.Remove(m, DnDCondition.Unconscious);

			if (m.Hits < 1)
			{
				m.Hits = 1;
			}

			if (announce)
			{
				m.SendMessage(0x40, "You are back on your feet.");
			}

			m.PublicOverheadMessage(Network.MessageType.Regular, 0x40, false, m.Name + " rises.");
		}

		private static void Die(Mobile m)
		{
			Stop(m);

			m.Frozen = false;

			DnDConditions.Remove(m, DnDCondition.Unconscious);

			m.SendMessage(0x22, "You have died.");

			// Kill() runs OnBeforeDeath again; the failure count has been cleared by Stop, so the
			// guard there would restart dying. Mark it first so the death goes through.
			m_Dying[m] = new DyingState { Failures = Needed };

			m.Kill();

			m_Dying.Remove(m);
		}

		private static void Stop(Mobile m)
		{
			DyingState state;

			if (m != null && m_Dying.TryGetValue(m, out state))
			{
				if (state.Timer != null)
				{
					state.Timer.Stop();
				}

				m_Dying.Remove(m);
			}
		}

		/// <summary>A long rest clears any lingering dying state, as does a resurrection.</summary>
		public static void Clear(Mobile m)
		{
			Stop(m);

			if (m != null)
			{
				m.Frozen = false;
				DnDConditions.Remove(m, DnDCondition.Unconscious);
			}
		}
	}
}
