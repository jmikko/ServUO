using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Classes
{
	public static class ClassSystem
	{
		/// <summary>
		/// Invoked automatically by ScriptCompiler.Invoke("Configure") at server boot, before
		/// Region.Load()/World.Load() run — mirrors Scripts/Misc/RaceDefinitions.cs' Configure()
		/// timing, so CharacterClass.Parse() has classes registered before any saved PlayerMobile
		/// data deserializes. Class registration order is also the wire contract for
		/// DnDCharacterSetup's classIndex field and DnDStatSync's classId field - ClassicUO's
		/// DnDCharacterSetupGump hardcodes this exact same order client-side (no name-based lookup
		/// over the network). Keep both in sync.
		/// </summary>
		public static void Configure()
		{
			CommandSystem.Register("DnDSheet", AccessLevel.Player, OnDnDSheetCommand);

			CharacterClass.Register(new FighterClass());
			CharacterClass.Register(new BarbarianClass());
			CharacterClass.Register(new BardClass());
			CharacterClass.Register(new ClericClass());
			CharacterClass.Register(new DruidClass());
			CharacterClass.Register(new MonkClass());
			CharacterClass.Register(new PaladinClass());
			CharacterClass.Register(new RangerClass());
			CharacterClass.Register(new RogueClass());
			CharacterClass.Register(new SorcererClass());
			CharacterClass.Register(new WarlockClass());
			CharacterClass.Register(new WizardClass());

			EventSink.DnDCharacterSetup += OnDnDCharacterSetup;
		}

		/// <summary>
		/// Applies the ability-score/class choices validated by PacketHandlers.DnDCharacterSetup and
		/// grants the Phase 1 vertical slice starting kit. Runs here (Scripts/) rather than in
		/// Server/Network/PacketHandlers.cs because PlayerMobile and the starting-kit items are
		/// Scripts-layer types Server.csproj cannot reference directly.
		/// </summary>
		private static void OnDnDCharacterSetup(DnDCharacterSetupEventArgs e)
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if (pm == null || pm.DnDInitialized)
			{
				return;
			}

			CharacterClass charClass = CharacterClass.AllClasses[e.ClassIndex];

			// Apply species ability score bonuses on top of the client-submitted base scores, and
			// let the D&D setup screen's species pick override whatever race vanilla character
			// creation assigned (needed for the six new SRD species, which the stock creation UI
			// has no buttons for at all - see Race.Races[e.SpeciesIndex] validation in
			// PacketHandlers.DnDCharacterSetup).
			Race species = Race.Races[e.SpeciesIndex];
			IDnDSpecies speciesTraits = species as IDnDSpecies;

			AbilityScores baseScores = e.Scores;

			AbilityScores finalScores = speciesTraits == null
				? baseScores
				: new AbilityScores(
					baseScores.Str + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Str),
					baseScores.Dex + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Dex),
					baseScores.Con + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Con),
					baseScores.Int + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Int),
					baseScores.Wis + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Wis),
					baseScores.Cha + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Cha));

			if (Core.Expansion >= species.RequiredExpansion)
			{
				pm.Race = species; // sets body too
			}

			pm.ApplyDnDSetup(finalScores, charClass);

			// A single hard-coded martial kit only suits proficient classes (Fighter, Barbarian,
			// Paladin, Ranger); only equip what the class can actually use so casters/Rogues/
			// Monks don't get handed gear that just bounces off their own proficiency gate. A
			// real per-class starting-kit table (staves for casters, etc.) is future content work.
			DnDLongsword sword = new DnDLongsword();

			if (charClass.IsProficientWith(sword))
			{
				pm.EquipItem(sword);
			}
			else
			{
				sword.Delete();
			}

			DnDChainShirt armor = new DnDChainShirt();

			if (charClass.IsProficientWith(armor))
			{
				pm.EquipItem(armor);
			}
			else
			{
				armor.Delete();
			}

			if (pm.NetState != null)
			{
				pm.NetState.Send(new DnDStatSync(pm));
			}
		}

		/// <summary>
		/// [DnDSheet - reopens/refreshes the caller's D&amp;D character sheet panel. Just resends the
		/// same DnDStatSync packet character setup does; the client already (re-)creates the panel
		/// whenever one arrives, so no new client-side plumbing is needed for this.
		/// </summary>
		private static void OnDnDSheetCommand(CommandEventArgs e)
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if (pm == null || !pm.DnDInitialized)
			{
				e.Mobile.SendMessage("You don't have a D&D character sheet - you haven't gone through D&D character setup.");
				return;
			}

			if (pm.NetState != null)
			{
				pm.NetState.Send(new DnDStatSync(pm));
			}
		}
	}
}
