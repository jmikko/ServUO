using System;
using System.Collections.Generic;

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
		protected virtual void Think()
		{
			if (Deleted || Map == null || Map == Map.Internal)
			{
				StopThinking();
				return;
			}

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
