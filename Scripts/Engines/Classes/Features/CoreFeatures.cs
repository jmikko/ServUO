using System;

namespace Server.Engines.Classes.Features
{
	/// <summary>
	/// SRD Extra Attack: at 5th level a martial class attacks twice with one attack action. This is
	/// most of what separates a 5th-level fighter from a 1st-level one - roughly a doubling of
	/// damage output that no amount of extra hit points or proficiency conveys.
	/// </summary>
	public sealed class ExtraAttackFeature : ClassFeature
	{
		public override string Name { get { return "Extra Attack"; } }
		public override int Level { get { return 5; } }
		public override int ExtraAttacks { get { return 1; } }

		public override string Description
		{
			get { return "You attack twice whenever you take the Attack action."; }
		}
	}

	/// <summary>
	/// SRD Sneak Attack: a Rogue adds 1d6 per two levels, rounded up, when they have advantage.
	/// <para>
	/// The SRD also allows it when an ally is adjacent to the target, which needs a notion of
	/// allies this server does not have yet - so for now it keys off advantage alone. That makes
	/// it strictly narrower than the rules rather than broader, which is the safer direction to be
	/// wrong in.
	/// </para>
	/// </summary>
	public sealed class SneakAttackFeature : ClassFeature
	{
		public override string Name { get { return "Sneak Attack"; } }
		public override int Level { get { return 1; } }

		public override string Description
		{
			get { return "Extra damage when you attack with advantage."; }
		}

		public override string GetBonusDamage(IDnDCharacter character, int classLevel, RollMode mode)
		{
			if (mode != RollMode.Advantage)
			{
				return null;
			}

			// 1d6 at 1st and 2nd, 2d6 at 3rd and 4th, and so on.
			int dice = (classLevel + 1) / 2;

			return dice > 0 ? dice + "d6" : null;
		}
	}

	/// <summary>
	/// SRD Improved Critical, the Champion's 3rd-level feature: critical hits land on a 19 as well
	/// as a 20, doubling how often the damage dice are rolled twice.
	/// </summary>
	public sealed class ImprovedCriticalFeature : ClassFeature
	{
		public override string Name { get { return "Improved Critical"; } }
		public override int Level { get { return 3; } }
		public override int CriticalThreshold { get { return 19; } }

		public override string Description
		{
			get { return "Your attacks score a critical hit on a roll of 19 or 20."; }
		}
	}
}
