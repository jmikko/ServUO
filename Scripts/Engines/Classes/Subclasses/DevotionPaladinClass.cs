using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Paladin subclass: the oath of the ideal knight.</summary>
	public class DevotionPaladinClass : PaladinClass
	{
		public override string Name { get { return "Oath of Devotion"; } }

		public override Type ParentClass { get { return typeof(PaladinClass); } }
	}
}
