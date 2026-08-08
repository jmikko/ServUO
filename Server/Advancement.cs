#region References
using System;
using System.Globalization;
#endregion

namespace Server
{
	/// <summary>
	/// Character advancement: the SRD experience table, the challenge-rating to experience award
	/// table, and the hit points a level is worth.
	/// <para>
	/// This replaces UO's skill-gain model entirely. Characters do not improve by using things;
	/// they improve by earning experience and crossing a threshold, which is what makes proficiency
	/// bonus, hit points, spell slots and cantrip damage all move at once.
	/// </para>
	/// </summary>
	public static class Advancement
	{
		public const int MaxLevel = 20;

		/// <summary>Total experience needed to reach each level; index 0 is level 1.</summary>
		private static readonly int[] ExperienceThresholds =
		{
			0,      // 1
			300,    // 2
			900,    // 3
			2700,   // 4
			6500,   // 5
			14000,  // 6
			23000,  // 7
			34000,  // 8
			48000,  // 9
			64000,  // 10
			85000,  // 11
			100000, // 12
			120000, // 13
			140000, // 14
			165000, // 15
			195000, // 16
			225000, // 17
			265000, // 18
			305000, // 19
			355000  // 20
		};

		/// <summary>
		/// Experience awarded for defeating a monster of a given challenge rating. Fractional
		/// ratings are keyed by their eighths so 1/8, 1/4 and 1/2 all land exactly.
		/// </summary>
		private static readonly double[] FractionalRatings = { 0.0, 0.125, 0.25, 0.5 };

		private static readonly int[] FractionalExperience = { 10, 25, 50, 100 };

		/// <summary>Experience by whole challenge rating, index 0 being CR 1.</summary>
		private static readonly int[] WholeRatingExperience =
		{
			200,    // 1
			450,    // 2
			700,    // 3
			1100,   // 4
			1800,   // 5
			2300,   // 6
			2900,   // 7
			3900,   // 8
			5000,   // 9
			5900,   // 10
			7200,   // 11
			8400,   // 12
			10000,  // 13
			11500,  // 14
			13000,  // 15
			15000,  // 16
			18000,  // 17
			20000,  // 18
			22000,  // 19
			25000,  // 20
			33000,  // 21
			41000,  // 22
			50000,  // 23
			62000,  // 24
			75000,  // 25
			90000,  // 26
			105000, // 27
			120000, // 28
			135000, // 29
			155000  // 30
		};

		public static int GetLevelForExperience(int experience)
		{
			int level = 1;

			for (int i = 1; i < ExperienceThresholds.Length; ++i)
			{
				if (experience >= ExperienceThresholds[i])
				{
					level = i + 1;
				}
				else
				{
					break;
				}
			}

			return level;
		}

		public static int GetExperienceForLevel(int level)
		{
			int index = Math.Max(1, Math.Min(MaxLevel, level)) - 1;

			return ExperienceThresholds[index];
		}

		/// <summary>Experience still needed to reach the next level, or 0 at the cap.</summary>
		public static int GetExperienceToNextLevel(int experience)
		{
			int level = GetLevelForExperience(experience);

			if (level >= MaxLevel)
			{
				return 0;
			}

			return GetExperienceForLevel(level + 1) - experience;
		}

		public static int GetExperienceForChallengeRating(double challengeRating)
		{
			if (challengeRating < 1.0)
			{
				int best = FractionalExperience[0];

				// Round to the nearest defined fraction rather than trusting exact equality on a
				// value that came out of an XML attribute.
				double closest = double.MaxValue;

				for (int i = 0; i < FractionalRatings.Length; ++i)
				{
					double distance = Math.Abs(FractionalRatings[i] - challengeRating);

					if (distance < closest)
					{
						closest = distance;
						best = FractionalExperience[i];
					}
				}

				return best;
			}

			int index = (int)Math.Round(challengeRating) - 1;

			return WholeRatingExperience[Math.Min(index, WholeRatingExperience.Length - 1)];
		}

		/// <summary>
		/// Total maximum hit points for a class at a level: the full hit die at 1st, then the die's
		/// average (rounded up) for each level after, with the Constitution modifier every level.
		/// SRD offers rolling as an alternative; taking the fixed average keeps a character's hit
		/// points reproducible from their sheet alone.
		/// </summary>
		public static int GetMaxHitPoints(int hitDie, int conModifier, int level)
		{
			level = Math.Max(1, level);

			int perLevelAverage = (hitDie / 2) + 1;
			int total = hitDie + conModifier + ((level - 1) * (perLevelAverage + conModifier));

			return Math.Max(level, total); // never below 1 hit point per level
		}

		public static double ParseChallengeRating(string value)
		{
			double result;

			if (String.IsNullOrEmpty(value) ||
				!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
			{
				return 0.0;
			}

			return result;
		}

		/// <summary>Formats a challenge rating the way a stat block writes it: 1/4, 1/2, 5.</summary>
		public static string FormatChallengeRating(double challengeRating)
		{
			if (challengeRating >= 1.0 || challengeRating <= 0.0)
			{
				return ((int)Math.Round(challengeRating)).ToString(CultureInfo.InvariantCulture);
			}

			return String.Format(CultureInfo.InvariantCulture, "1/{0}", (int)Math.Round(1.0 / challengeRating));
		}
	}
}
