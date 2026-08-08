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
		CharacterClass CharacterClass { get; }
		int CharacterLevel { get; }
		int ArmorClass { get; }
		int Experience { get; }

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
