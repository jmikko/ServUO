using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Cleric subclass: the healer's calling.</summary>
	public class LifeDomainClass : ClericClass
	{
		public override string Name { get { return "Life Domain"; } }

		public override Type ParentClass { get { return typeof(ClericClass); } }

		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.ChannelDivinityFeature(), new Features.DiscipleOfLifeFeature() }; }
		}
	}
}
