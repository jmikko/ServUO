using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Bard subclass: knowledge gathered from everywhere and used against everyone.</summary>
	public class LoreBardClass : BardClass
	{
		public override string Name { get { return "College of Lore"; } }

		public override Type ParentClass { get { return typeof(BardClass); } }

		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.BardicInspirationFeature(), new Features.CuttingWordsFeature() }; }
		}
	}
}
