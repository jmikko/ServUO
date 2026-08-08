using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Barbarian subclass: rage carried to the point of frenzy.</summary>
	public class BerserkerClass : BarbarianClass
	{
		public override string Name { get { return "Path of the Berserker"; } }

		public override Type ParentClass { get { return typeof(BarbarianClass); } }
	}
}
