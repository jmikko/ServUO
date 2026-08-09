using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Rogue subclass: speed, stealth and other people's property.</summary>
	public class ThiefRogueClass : RogueClass
	{
		public override string Name { get { return "Thief"; } }

		public override Type ParentClass { get { return typeof(RogueClass); } }

		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.SneakAttackFeature(), new Features.CunningActionFeature(), new Features.UncannyDodgeFeature(), new Features.EvasionFeature(), new Features.FastHandsFeature() }; }
		}
	}
}
