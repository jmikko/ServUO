using System;
using System.Collections.Generic;

namespace Server
{
	/// <summary>What a combatant can spend in one round.</summary>
	[Flags]
	public enum TurnResource
	{
		None = 0x00,

		/// <summary>The main thing you do: attack, cast, dash.</summary>
		Action = 0x01,

		/// <summary>The smaller thing you can add to it, if a feature grants one.</summary>
		BonusAction = 0x02,

		/// <summary>The one thing you may do outside your own turn.</summary>
		Reaction = 0x04
	}

	/// <summary>
	/// A turn economy for a game that has no turns.
	/// <para>
	/// This is the decision the design had been deferring, made one way rather than the other: this
	/// stays a real-time game with D&amp;D's numbers, not a turn-based port. Everything already built
	/// - the swing timer, movement, spawning - assumes real time, and turning that inside out would
	/// mean rebuilding all of it to gain a pause between swings that nobody asked for.
	/// </para>
	/// <para>
	/// So a "turn" here is a six-second window that refills on its own. You get one action, one
	/// bonus action and one reaction per round, and they come back whether or not you used them.
	/// The SRD's ordering is lost - you cannot hold a reaction for a specific trigger - but the
	/// scarcity is kept, and scarcity is what those features are actually about. Uncanny Dodge
	/// mattering once per round is most of Uncanny Dodge.
	/// </para>
	/// <para>
	/// Reactions are the interesting case, because unlike an action nobody spends one deliberately:
	/// the rules spend it for you when something happens. <see cref="TrySpendReaction"/> is that -
	/// a rule asks whether the reaction is available, and consumes it if so, in one call. Making it
	/// two calls would let a caller check and then forget to spend.
	/// </para>
	/// </summary>
	public static class DnDTurn
	{
		/// <summary>One SRD round.</summary>
		public static readonly TimeSpan RoundLength = TimeSpan.FromSeconds(6.0);

		private sealed class TurnState
		{
			public DateTime RoundStarted;
			public TurnResource Spent;
		}

		private static readonly Dictionary<Mobile, TurnState> m_Turns = new Dictionary<Mobile, TurnState>();

		/// <summary>
		/// The state for this mobile, rolled over to a fresh round if the last one has elapsed.
		/// <para>
		/// Rolling over lazily rather than on a timer means there is no per-mobile timer to leak,
		/// and a mobile nobody is asking about costs nothing. The cost is that "rounds" are measured
		/// from each combatant's first spend rather than from a global clock - which is right for a
		/// real-time game, where there is no global clock to measure from.
		/// </para>
		/// </summary>
		private static TurnState GetState(Mobile m)
		{
			TurnState state;

			if (!m_Turns.TryGetValue(m, out state))
			{
				state = new TurnState { RoundStarted = DateTime.UtcNow };
				m_Turns[m] = state;

				return state;
			}

			if (DateTime.UtcNow - state.RoundStarted >= RoundLength)
			{
				state.RoundStarted = DateTime.UtcNow;
				state.Spent = TurnResource.None;
			}

			return state;
		}

		public static bool IsAvailable(Mobile m, TurnResource resource)
		{
			if (m == null)
			{
				return false;
			}

			return (GetState(m).Spent & resource) == 0;
		}

		/// <summary>
		/// Spends a resource if it is available, reporting whether it was. One call rather than a
		/// check and a spend, so a rule cannot consult the reaction and then fail to use it up.
		/// </summary>
		public static bool TrySpend(Mobile m, TurnResource resource)
		{
			if (m == null)
			{
				return false;
			}

			TurnState state = GetState(m);

			if ((state.Spent & resource) != 0)
			{
				return false;
			}

			state.Spent |= resource;

			return true;
		}

		/// <summary>The common case: a rule wants to fire in response to something.</summary>
		public static bool TrySpendReaction(Mobile m)
		{
			return TrySpend(m, TurnResource.Reaction);
		}

		public static bool TrySpendBonusAction(Mobile m)
		{
			return TrySpend(m, TurnResource.BonusAction);
		}

		public static bool TrySpendAction(Mobile m)
		{
			return TrySpend(m, TurnResource.Action);
		}

		/// <summary>Gives everything back - used when combat ends and on a rest.</summary>
		public static void Reset(Mobile m)
		{
			if (m != null)
			{
				m_Turns.Remove(m);
			}
		}

		/// <summary>How long until this mobile's resources come back, for a UI to show.</summary>
		public static TimeSpan TimeUntilRefresh(Mobile m)
		{
			TurnState state;

			if (m == null || !m_Turns.TryGetValue(m, out state))
			{
				return TimeSpan.Zero;
			}

			TimeSpan elapsed = DateTime.UtcNow - state.RoundStarted;

			return elapsed >= RoundLength ? TimeSpan.Zero : RoundLength - elapsed;
		}
	}
}
