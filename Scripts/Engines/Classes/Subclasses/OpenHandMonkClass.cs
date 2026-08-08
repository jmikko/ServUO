using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Monk subclass: the body as the whole of the art.</summary>
	public class OpenHandMonkClass : MonkClass
	{
		public override string Name { get { return "Way of the Open Hand"; } }

		public override Type ParentClass { get { return typeof(MonkClass); } }
	}
}
