using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Warlock subclass: a bargain struck with something from the lower planes.</summary>
	public class FiendWarlockClass : WarlockClass
	{
		public override string Name { get { return "The Fiend"; } }

		public override Type ParentClass { get { return typeof(WarlockClass); } }
	}
}
