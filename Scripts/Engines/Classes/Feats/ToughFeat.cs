using System;

namespace Server.Engines.Classes.Feats
{
	/// <summary>
	/// Tough: +2 hit points per level, which keeps growing as you do.
	/// <para>
	/// The hit point total is now read through <see cref="Feat.GetHitPointsPerLevel"/> rather than
	/// by naming this class at the call site, so any later feat that grants hit points needs no
	/// change to HitsMax.
	/// </para>
	/// </summary>
	public class ToughFeat : Feat
	{
		public override string Name { get { return "Tough"; } }

		public override string Description
		{
			get { return "Your hit point maximum increases by 2 for every level you have."; }
		}

		public override int HitPointsPerLevel { get { return 2; } }
	}
}
