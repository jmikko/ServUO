using System;

namespace Server.Engines.Classes.Features
{
	/// <summary>
	/// Wild Shape: a Druid takes the form of a beast.
	/// <para>
	/// Twice per short rest, which is the limit the SRD puts on it, and capped by challenge rating
	/// so it grows with the Druid rather than arriving whole at 2nd level. Which beast is an
	/// argument - <c>[use Wild Shape wolf</c> - because a feature that picked for you would not be
	/// Wild Shape.
	/// </para>
	/// </summary>
	public sealed class WildShapeFeature : ClassFeature
	{
		public override string Name { get { return "Wild Shape"; } }
		public override int Level { get { return 2; } }

		public override string Description
		{
			get { return "Take the form of a beast you have seen. Twice per short rest."; }
		}

		public override int GetUses(int classLevel) { return 2; }

		public override bool RecoversOnShortRest { get { return true; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			// Already shaped: this is how you come back early.
			if (Mobiles.DnDWildShape.IsShaped(user))
			{
				Mobiles.DnDWildShape.Revert(user, null);

				// Reverting deliberately costs no use - it is not a second transformation.
				return false;
			}

			double cap = Mobiles.DnDWildShape.GetMaxChallengeRating(classLevel);

			string form = LastRequestedForm;

			if (String.IsNullOrEmpty(form))
			{
				user.SendMessage("Name a form: [use Wild Shape <beast>.");
				ListForms(user, cap);

				return false;
			}

			// One hour per two Druid levels, as the rules have it.
			TimeSpan duration = TimeSpan.FromHours(Math.Max(1, classLevel / 2));

			return Mobiles.DnDWildShape.Assume(user, form, cap, duration);
		}

		private static void ListForms(Mobile user, double cap)
		{
			var forms = Mobiles.DnDWildShape.GetAvailableForms(cap);

			if (forms.Count == 0)
			{
				user.SendMessage("You know no forms of challenge {0} or less.", cap);
				return;
			}

			user.SendMessage(0x35, "Forms available to you (challenge {0} or less):", cap);

			foreach (Mobiles.SrdMonsterData data in forms)
			{
				user.SendMessage("{0} - AC {1}, {2} hit points", data.Id, data.ArmorClass, data.HitPoints);
			}
		}

		/// <summary>
		/// The form named on the command line, handed over by [use before Activate runs.
		/// <para>
		/// A static rather than a parameter because <c>Activate</c> is the shared shape of every
		/// activated feature, and widening it for the one feature that takes an argument would
		/// touch every other one. Single-threaded command handling makes this safe.
		/// </para>
		/// </summary>
		public static string LastRequestedForm { get; set; }
	}
}
