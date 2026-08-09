using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Paladin subclass: the oath of the ideal knight.</summary>
	public class DevotionPaladinClass : PaladinClass
	{
		public override string Name { get { return "Oath of Devotion"; } }

		public override Type ParentClass { get { return typeof(PaladinClass); } }

		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.LayOnHandsPoolFeature(), new Features.ExtraAttackFeature(), new Features.AuraOfProtectionFeature(), new Features.DivineSmiteFeature(), new Features.SacredWeaponFeature() }; }
		}
	}
}
