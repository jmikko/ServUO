using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Druid subclass: magic drawn from a particular country.</summary>
	public class LandDruidClass : DruidClass
	{
		public override string Name { get { return "Circle of the Land"; } }

		public override Type ParentClass { get { return typeof(DruidClass); } }

		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.WildShapeFeature(), new Features.NaturalRecoveryFeature() }; }
		}
	}
}
