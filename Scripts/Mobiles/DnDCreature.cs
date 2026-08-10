using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	/// <summary>
	/// How a creature picks fights. Deliberately far simpler than UO's FightMode - D&D monsters
	/// are either hostile to players on sight, or only retaliate.
	/// </summary>
	public enum DnDAggression
	{
		/// <summary>Attacks any player who comes within <see cref="DnDCreature.AggroRange"/>.</summary>
		Hostile,

		/// <summary>Ignores players until damaged, then fights back.</summary>
		Defensive
	}

	/// <summary>
	/// Base class for every D&amp;D creature.
	/// <para>
	/// This deliberately does NOT derive from (or port) ServUO's BaseCreature. That class carries
	/// taming, bonding, pack instincts, barding, loot tables, control slots, spell schools and a
	/// large AI hierarchy - all of it legacy UO content with no D&amp;D equivalent, and all of it
	/// transitively pulling in most of the original Scripts tree.
	/// </para>
	/// <para>
	/// Instead this sits directly on <see cref="Mobile"/> (which already provides movement,
	/// damage, corpse creation and the ISpawnable contract the region spawner needs) and adds only
	/// what a D&amp;D stat block requires: hit points, an armour class, an attack bonus, a damage
	/// dice expression, and a simple aggro/pursue/attack think loop.
	/// </para>
	/// Combat resolution itself lives in the rules core (BaseWeapon.CheckHit / ComputeDamage read
	/// <see cref="IDnDCreature"/>), so this class only has to expose the numbers.
	/// </summary>
	public abstract class DnDCreature : Mobile, IDnDCreature
	{
		/// <summary>
		/// Nothing here heals just by standing still.
		/// <para>
		/// Mobile runs a HitsTimer that ticks a hit point back at UO's default rate for every
		/// creature in the game, and nothing in this rebuild had ever turned it off - so a troll
		/// left alone quietly refilled, and so did everything else, including players. That is UO's
		/// attrition model, and it is the opposite of D&amp;D's: hit points are meant to be a
		/// resource spent across a day and recovered at a rest or from a spell, which is what makes
		/// hit dice and healing worth anything.
		/// </para>
		/// <para>
		/// Creatures with the Regeneration trait get theirs from the trait, on their own terms.
		/// </para>
		/// </summary>
		public override bool CanRegenHits { get { return false; } }

		private static readonly TimeSpan ThinkInterval = TimeSpan.FromSeconds(0.5);

		private Timer m_ThinkTimer;

		[CommandProperty(AccessLevel.GameMaster)]
		public DnDAggression Aggression { get; set; }

		/// <summary>How far this creature notices hostile players from, in tiles.</summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public int AggroRange { get; set; }

		/// <summary>Spawn anchor, seeded by the region spawner.</summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D Home { get; set; }

		/// <summary>How far it will wander from <see cref="Home"/> before returning.</summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public int RangeHome { get; set; }

		#region IDnDCreature - the stat block
		[CommandProperty(AccessLevel.GameMaster)]
		public virtual int ArmorClass { get { return 10; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public virtual int AttackBonus { get { return 0; } }

		public virtual string DamageDiceExpression { get { return "1d4"; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public virtual int HitPointsMaxDnD { get { return 1; } }

		/// <summary>SRD challenge rating - the sole source of the experience this creature is worth.</summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public virtual double ChallengeRating { get { return 0.0; } }

		public int ExperienceValue { get { return Advancement.GetExperienceForChallengeRating(ChallengeRating); } }
		#endregion

		protected DnDCreature(DnDAggression aggression)
		{
			Aggression = aggression;
			AggroRange = 10;
			RangeHome = 10;

			Direction = Direction.South;
		}

		protected DnDCreature(Serial serial)
			: base(serial)
		{
		}

		public override bool ShouldCheckStatTimers { get { return false; } }

		/// <summary>D&D hit points replace UO's Str-derived HitsMax entirely.</summary>
		public override int HitsMax { get { return HitPointsMaxDnD; } }

		/// <summary>Monsters are always hostile to players for notoriety purposes.</summary>
		public virtual bool IsMonster { get { return true; } }

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();

			if (Home == Point3D.Zero)
			{
				Home = Location;
			}

			StartThinking();
		}

		protected override void OnMapChange(Map oldMap)
		{
			base.OnMapChange(oldMap);

			if (Map == Map.Internal || Deleted)
			{
				StopThinking();
			}
			else
			{
				StartThinking();
			}
		}

		protected void StartThinking()
		{
			if (m_ThinkTimer == null && !Deleted && Map != null && Map != Map.Internal)
			{
				m_ThinkTimer = Timer.DelayCall(ThinkInterval, ThinkInterval, Think);
			}
		}

		protected void StopThinking()
		{
			if (m_ThinkTimer != null)
			{
				m_ThinkTimer.Stop();
				m_ThinkTimer = null;
			}
		}

		/// <summary>
		/// One AI tick: keep a valid target, close to melee range, and swing.
		/// Mobile's own combat timer drives the actual swing once Combatant is set and we are
		/// adjacent, so this only has to handle target selection and movement.
		/// </summary>
		private DateTime m_NextRegeneration;

		/// <summary>
		/// The Regeneration trait: hit points back at the start of each of the creature's turns.
		/// <para>
		/// Once per round rather than per think tick, because the trait is written per turn and a
		/// half-second tick would hand a troll twelve times its stated rate. It stops at nothing
		/// left to heal, and does not raise the dead - a creature reduced to 0 is finished, which
		/// is what makes killing a troll possible at all.
		/// </para>
		/// </summary>
		private void ApplyRegeneration()
		{
			var traited = this as IDnDTraited;

			if (traited == null || traited.Traits == null || traited.Traits.Regeneration <= 0)
			{
				return;
			}

			if (!Alive || Hits >= HitsMax)
			{
				return;
			}

			if (DateTime.UtcNow < m_NextRegeneration)
			{
				return;
			}

			m_NextRegeneration = DateTime.UtcNow + Mobiles.DnDDeath.RoundLength;

			Hits = Math.Min(HitsMax, Hits + traited.Traits.Regeneration);
		}

		protected virtual void Think()
		{
			if (Deleted || Map == null || Map == Map.Internal)
			{
				StopThinking();
				return;
			}

			ApplyRegeneration();

			if (!Alive)
			{
				return;
			}

			Mobile target = Combatant as Mobile;

			if (!IsValidTarget(target))
			{
				target = Aggression == DnDAggression.Hostile ? AcquireTarget() : null;
				Combatant = target;
			}

			if (target == null)
			{
				ReturnHome();
				return;
			}

			if (!InRange(target.Location, 1))
			{
				Direction = GetDirectionTo(target);
				Move(Direction);
			}
			else
			{
				Direction = GetDirectionTo(target);
				SelectAndExecuteAction(target);
			}
		}

		private DateTime m_NextActionTime;

		protected virtual void SelectAndExecuteAction(Mobile target)
		{
			if (DateTime.UtcNow < m_NextActionTime)
			{
				return;
			}

			// Throttle actions to roughly once per 3 seconds (standard UO combat swing speed)
			m_NextActionTime = DateTime.UtcNow + TimeSpan.FromSeconds(3.0);

			SrdMonster srd = this as SrdMonster;
			if (srd == null || srd.Actions == null || srd.Actions.Count == 0)
			{
				// Fallback to standard UO Combatant-driven melee swings
				return; 
			}

			// Priority 1: Check Limited Usage Actions (e.g. Breath Weapons)
			foreach (DnDAction action in srd.Actions)
			{
				if (action.Usage == DnDUsageType.Recharge5_6 || action.Usage == DnDUsageType.Recharge6)
				{
					// Simple recharge roll
					int roll = Utility.RandomMinMax(1, 6);
					if ((action.Usage == DnDUsageType.Recharge5_6 && roll >= 5) ||
						(action.Usage == DnDUsageType.Recharge6 && roll == 6))
					{
						ExecuteAction(action, target);
						return;
					}
				}
			}

			// Priority 2: Standard Multiattack or Basic Melee
			// For now, the BaseWeapon combat timer handles standard melee, so if we 
			// reach this point, we just do nothing and let the Combatant system swing.
		}

		protected virtual void ExecuteAction(DnDAction action, Mobile target)
		{
			// Skeleton for executing complex non-melee actions
			if (action.Type == DnDActionType.BreathWeapon || action.Type == DnDActionType.RangedSpell)
			{
				this.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, string.Format("*uses {0}*", action.Name));
				// TODO: Implement actual damage dealing / area of effect calculation
			}
		}

		protected virtual bool IsValidTarget(Mobile m)
		{
			return m != null
				&& !m.Deleted
				&& m.Alive
				&& !m.IsDeadBondedPet
				&& m.Map == Map
				&& m.AccessLevel == AccessLevel.Player
				&& InRange(m.Location, AggroRange)
				&& CanSee(m)
				&& InLOS(m);
		}

		/// <summary>Nearest valid player inside <see cref="AggroRange"/>.</summary>
		protected virtual Mobile AcquireTarget()
		{
			Mobile best = null;
			int bestDist = int.MaxValue;

			IPooledEnumerable eable = Map.GetMobilesInRange(Location, AggroRange);

			foreach (Mobile m in eable)
			{
				if (!m.Player || !IsValidTarget(m))
				{
					continue;
				}

				int dist = (int)GetDistanceToSqrt(m);

				if (dist < bestDist)
				{
					bestDist = dist;
					best = m;
				}
			}

			eable.Free();

			return best;
		}

		/// <summary>Drift back toward the spawn anchor when nothing is worth fighting.</summary>
		protected virtual void ReturnHome()
		{
			if (Home == Point3D.Zero || RangeHome <= 0 || InRange(Home, RangeHome))
			{
				return;
			}

			Direction = GetDirectionTo(Home);
			Move(Direction);
		}

		/// <summary>Defensive creatures wake up when hit.</summary>
		public override void AggressiveAction(Mobile aggressor, bool criminal)
		{
			base.AggressiveAction(aggressor, criminal);

			if (Combatant == null && IsValidTarget(aggressor))
			{
				Combatant = aggressor;
			}
		}

		public override void OnDelete()
		{
			StopThinking();

			base.OnDelete();
		}

		public override void OnAfterDelete()
		{
			StopThinking();

			base.OnAfterDelete();
		}

		/// <summary>
		/// Hands out experience. Everyone who damaged this creature is paid the full award rather
		/// than a share of it - that is how a D&amp;D party works, and splitting it would punish
		/// players for helping each other.
		/// </summary>
		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			int award = ExperienceValue;

			if (award <= 0)
			{
				return;
			}

			var paid = new List<Mobile>();

			foreach (DamageEntry entry in DamageEntries)
			{
				IDnDCharacter character = entry.Damager as IDnDCharacter;

				if (character == null || entry.HasExpired || paid.Contains(entry.Damager))
				{
					continue;
				}

				paid.Add(entry.Damager);
				character.AwardExperience(award);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write((int)Aggression);
			writer.Write(AggroRange);
			writer.Write(Home);
			writer.Write(RangeHome);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt(); // version

			Aggression = (DnDAggression)reader.ReadInt();
			AggroRange = reader.ReadInt();
			Home = reader.ReadPoint3D();
			RangeHome = reader.ReadInt();

			StartThinking();
		}
	}
}
