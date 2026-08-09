using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Ranger subclass: the specialist in dangerous prey.</summary>
	public class HunterRangerClass : RangerClass
	{
		public override string Name { get { return "Hunter"; } }

		public override Type ParentClass { get { return typeof(RangerClass); } }

		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.ExtraAttackFeature(), new Features.ColossusSlayerFeature() }; }
		}
	}
}
