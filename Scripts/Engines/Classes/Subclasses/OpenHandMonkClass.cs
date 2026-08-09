using System;

namespace Server.Engines.Classes.Subclasses
{
	/// <summary>SRD Monk subclass: the body as the whole of the art.</summary>
	public class OpenHandMonkClass : MonkClass
	{
		public override string Name { get { return "Way of the Open Hand"; } }

		public override Type ParentClass { get { return typeof(MonkClass); } }

		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.MonkUnarmoredDefenseFeature(), new Features.MartialArtsFeature(), new Features.ExtraAttackFeature(), new Features.FlurryOfBlowsFeature(), new Features.PatientDefenseFeature(), new Features.StepOfTheWindFeature(), new Features.StunningStrikeFeature(), new Features.DeflectMissilesFeature(), new Features.EvasionFeature(), new Features.OpenHandTechniqueFeature() }; }
		}
	}
}
