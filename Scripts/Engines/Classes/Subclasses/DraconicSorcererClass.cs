using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Sorcerer subclass: magic inherited from a dragon.</summary>
	public class DraconicSorcererClass : SorcererClass
	{
		public override string Name { get { return "Draconic Bloodline"; } }

		public override Type ParentClass { get { return typeof(SorcererClass); } }
	}
}
