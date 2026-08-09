using System.Collections.Generic;

namespace Server
{
	/// <summary>
	/// Implemented by PlayerMobile (in Scripts/) to expose D&amp;D 5.5e character state to Server/
	/// without Server/ taking a dependency on the Scripts-layer type, mirroring how Mobile keeps
	/// its Guild/Party references as untyped object for the same reason.
	/// </summary>
	public interface IDnDCharacter
	{
		bool DnDInitialized { get; }
		AbilityScores AbilityScores { get; }
		AbilityScores EffectiveAbilityScores { get; }
		IReadOnlyDictionary<CharacterClass, int> Classes { get; }

		/// <summary>
		/// The class this character started as. Multiclassing grants saving throw proficiencies
		/// from the first class only, so the engine needs to know which one that was - the Classes
		/// dictionary cannot answer it, since a dictionary has no first entry.
		/// </summary>
		CharacterClass PrimaryClass { get; }

		/// <summary>
		/// Levels across every class. Proficiency bonus derives from this rather than from any one
		/// class, which is what stops multiclassing being a way to farm proficiency.
		/// </summary>
		int TotalLevel { get; }
		System.Collections.Generic.List<Feat> Feats { get; }

		/// <summary>
		/// Names of the level-up options this character has chosen - fighting styles, expertise,
		/// invocations, pact boon, metamagic. Stored as names rather than objects so the save file
		/// does not depend on a registry that may be reordered.
		/// </summary>
		System.Collections.Generic.List<string> Choices { get; }
		int ArmorClass { get; }
		int Experience { get; }
		bool IsProficient(DnDSkill skill);
		bool IsAttunedTo(Item item);

		/// <summary>
		/// Adds experience and levels the character up if that crosses a threshold. Implemented in
		/// Scripts/ so Server/ never needs to know how hit points, spell slots and the client's view
		/// of the sheet are brought back into line afterwards.
		/// </summary>
		void AwardExperience(int amount);

		/// <summary>
		/// Applies a one-time D&amp;D 5.5e setup (ability scores + class, level fixed at 1) and sets
		/// DnDInitialized. Implemented by PlayerMobile; kept as a single method (rather than exposing
		/// property setters across the Server/Scripts boundary) so Server/ never needs to know how a
		/// PlayerMobile applies the resulting HP/state changes.
		/// </summary>
		void ApplyDnDSetup(AbilityScores scores, CharacterClass characterClass);
	}
}
