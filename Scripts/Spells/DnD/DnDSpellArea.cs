using System;
using System.Collections.Generic;

namespace Server.Spells.DnD
{
	/// <summary>
	/// The shapes an SRD area spell can take.
	/// <para>
	/// Sphere and Cube are placed on the target; Cone and Line originate at the caster and point at
	/// the target. That difference matters: a fireball is centred where you aim it, while a cone of
	/// flame comes out of you and only ever goes one way.
	/// </para>
	/// </summary>
	public enum SpellShape
	{
		Single,
		Sphere,
		Cone,
		Line,
		Cube
	}

	/// <summary>
	/// Works out which mobiles an area spell actually catches.
	/// <para>
	/// Sizes are in tiles, one tile being 5 feet - so the SRD's 15-foot cone is a size of 3.
	/// </para>
	/// </summary>
	public static class DnDSpellArea
	{
		/// <summary>
		/// A cone's width equals its length at any point along it, which works out to a half-angle
		/// whose tangent is 1/2. Comparing the perpendicular offset against half the distance along
		/// the axis is the same test without the trigonometry.
		/// </summary>
		private const double ConeSpread = 0.5;

		/// <summary>An SRD line is 5 feet wide - one tile, so half a tile either side of the axis.</summary>
		private const double LineHalfWidth = 0.5;

		/// <summary>
		/// Tile coordinates name a tile, not a point, so a creature anywhere in its square counts.
		/// Without this slack a cone visibly misses things sitting right on its edge.
		/// </summary>
		private const double TileSlack = 0.5;

		public static List<Mobile> GetTargets(Mobile caster, Mobile target, SpellShape shape, int size, bool beneficial)
		{
			var results = new List<Mobile>();

			if (caster == null || target == null)
			{
				return results;
			}

			if (shape == SpellShape.Single || size <= 0)
			{
				results.Add(target);
				return results;
			}

			Map map = caster.Map;

			if (map == null || map == Map.Internal)
			{
				results.Add(target);
				return results;
			}

			// Sphere and cube sit on the target; cone and line come out of the caster.
			bool originIsCaster = shape == SpellShape.Cone || shape == SpellShape.Line;
			Point3D origin = originIsCaster ? caster.Location : target.Location;

			double dirX = 0.0, dirY = 0.0;

			if (originIsCaster && !TryGetDirection(caster.Location, target.Location, out dirX, out dirY))
			{
				// Aiming a cone at your own feet has no direction to point it in.
				results.Add(target);
				return results;
			}

			foreach (Mobile m in map.GetMobilesInRange(origin, size + 1))
			{
				if (m == null || m.Deleted || !m.Alive)
				{
					continue;
				}

				// Every SRD cone, line and cube originates somewhere the caster is not, so a
				// harmful area never catches its own caster. A beneficial one may.
				if (m == caster && !beneficial)
				{
					continue;
				}

				if (Contains(shape, origin, dirX, dirY, size, m.Location))
				{
					results.Add(m);
				}
			}

			return results;
		}

		private static bool TryGetDirection(Point3D from, Point3D to, out double dirX, out double dirY)
		{
			double dx = to.X - from.X;
			double dy = to.Y - from.Y;
			double length = Math.Sqrt((dx * dx) + (dy * dy));

			if (length < 0.0001)
			{
				dirX = dirY = 0.0;
				return false;
			}

			dirX = dx / length;
			dirY = dy / length;

			return true;
		}

		private static bool Contains(
			SpellShape shape, Point3D origin, double dirX, double dirY, int size, Point3D point)
		{
			double dx = point.X - origin.X;
			double dy = point.Y - origin.Y;

			switch (shape)
			{
				case SpellShape.Sphere:
					{
						return Math.Sqrt((dx * dx) + (dy * dy)) <= size + TileSlack;
					}
				case SpellShape.Cube:
					{
						return Math.Abs(dx) <= size + TileSlack && Math.Abs(dy) <= size + TileSlack;
					}
				case SpellShape.Cone:
				case SpellShape.Line:
					{
						// Distance along the axis the spell points down, and offset to either side.
						double along = (dx * dirX) + (dy * dirY);
						double across = Math.Abs((dx * -dirY) + (dy * dirX));

						if (along < 0.0 || along > size + TileSlack)
						{
							return false; // behind the caster, or past the far end
						}

						if (shape == SpellShape.Line)
						{
							// A line is one tile wide, so only the tiles the axis actually crosses
							// are in it. Adding tile slack here would widen it to three.
							return across <= LineHalfWidth;
						}

						// A cone's boundary genuinely falls between tiles as it widens, so a
						// creature whose square the edge passes through is caught.
						return across <= (along * ConeSpread) + TileSlack;
					}
			}

			return false;
		}
	}
}
