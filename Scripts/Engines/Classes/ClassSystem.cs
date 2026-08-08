using System;
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

			// Subclasses, registered after their parents so GetParent() can find them. One per
			// class, which is what the SRD publishes - a character picks theirs at 3rd level and it
			// replaces the parent class in their record rather than adding to it.
			//
			// These were written but never registered, so they have never been offered to anyone.
			CharacterClass.Register(new Subclasses.ChampionClass());
			CharacterClass.Register(new Subclasses.BerserkerClass());
			CharacterClass.Register(new Subclasses.LoreBardClass());
			CharacterClass.Register(new Subclasses.LifeDomainClass());
			CharacterClass.Register(new Subclasses.LandDruidClass());
			CharacterClass.Register(new Subclasses.OpenHandMonkClass());
			CharacterClass.Register(new Subclasses.DevotionPaladinClass());
			CharacterClass.Register(new Subclasses.HunterRangerClass());
			CharacterClass.Register(new Subclasses.ThiefRogueClass());
			CharacterClass.Register(new Subclasses.DraconicSorcererClass());
			CharacterClass.Register(new Subclasses.FiendWarlockClass());
			CharacterClass.Register(new Subclasses.EvokerClass());

			EventSink.DnDCharacterSetup += OnDnDCharacterSetup;
			EventSink.DnDLevelUpSubmit += OnDnDLevelUpSubmit;
		}

		/// <summary>
		/// Applies the ability-score/class choices validated by PacketHandlers.DnDCharacterSetup and
		/// grants the Phase 1 vertical slice starting kit. Runs here (Scripts/) rather than in
		/// Server/Network/PacketHandlers.cs because PlayerMobile and the starting-kit items are
		/// Scripts-layer types Server.csproj cannot reference directly.
		/// </summary>
		private static void OnDnDCharacterSetup(DnDCharacterSetupEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

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

			// A species the client has no art for cannot be applied. Decide that BEFORE the ability
			// bonuses are worked out: applying the bonuses and then failing to set the race leaves
			// a character with, say, an Elf's Dexterity who is not an Elf.
			bool speciesAvailable = Core.Expansion >= species.RequiredExpansion;

			if (!speciesAvailable)
			{
				speciesTraits = null;

				pm.SendMessage(
					"Your shard does not support {0} characters; continuing without a species.", species.Name);

				Console.WriteLine(
					"ClassSystem: '{0}' requires expansion {1} but the shard is {2}; species not applied.",
					species.Name,
					species.RequiredExpansion,
					Core.Expansion);
			}

			AbilityScores finalScores = speciesTraits == null
				? baseScores
				: new AbilityScores(
					baseScores.Str + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Str),
					baseScores.Dex + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Dex),
					baseScores.Con + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Con),
					baseScores.Int + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Int),
					baseScores.Wis + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Wis),
					baseScores.Cha + speciesTraits.GetAbilityScoreBonus(AbilityScoreType.Cha));

			if (speciesAvailable)
			{
				pm.Race = species; // sets body too
			}

			pm.ApplyDnDSetup(finalScores, charClass);

			// After setup, since ApplyDnDSetup seeds the class defaults and this replaces them with
			// what the player actually chose.
			pm.ApplySkillProficiencies(charClass, e.Skills);

			GrantStartingKit(pm, charClass);

			if (pm.NetState != null)
			{
				pm.NetState.Send(new DnDStatSync(pm));
			}

			Spells.DnD.DnDSpellPackets.SendSpellList(pm);
		}

		/// <summary>
		/// Every class gets a weapon it can use on its first login. This is intentionally a small
		/// SRD baseline, not a substitute for class features or a full equipment economy.
		/// </summary>
		private static void GrantStartingKit(DnDPlayerMobile pm, CharacterClass charClass)
		{
			Item weapon;
			Item armor = null;

			switch (charClass.Name)
			{
				case "Fighter":
				case "Barbarian":
				case "Paladin":
					weapon = new DnDLongsword();
					armor = new DnDChainShirt();
					break;
				case "Ranger":
					weapon = new DnDShortbow();
					armor = new DnDChainShirt();
					break;
				case "Cleric":
				case "Druid":
					weapon = new DnDQuarterstaff();
					armor = new DnDChainShirt();
					break;
				case "Monk":
					weapon = new DnDQuarterstaff();
					break;
				case "Bard":
				case "Rogue":
				case "Warlock":
					weapon = new DnDDagger();
					armor = new DnDLeatherArmor();
					break;
				default: // Sorcerer and Wizard
					weapon = new DnDDagger();
					break;
			}

			EquipStartingItem(pm, weapon);

			if (armor != null)
			{
				EquipStartingItem(pm, armor);
			}
		}

		private static void EquipStartingItem(DnDPlayerMobile pm, Item item)
		{
			if (!pm.EquipItem(item))
			{
				pm.AddToBackpack(item);
			}
		}

		/// <summary>
		/// [DnDSheet - reopens/refreshes the caller's D&amp;D character sheet panel. Just resends the
		/// same DnDStatSync packet character setup does; the client already (re-)creates the panel
		/// whenever one arrives, so no new client-side plumbing is needed for this.
		/// </summary>
		private static void OnDnDSheetCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

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

		private static void OnDnDLevelUpSubmit(DnDLevelUpSubmitEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (pm == null || !pm.DnDInitialized)
			{
				return;
			}

			if (pm.PendingLevels > 0 && !string.IsNullOrEmpty(e.ChosenClass))
			{
				CharacterClass chosen = CharacterClass.Parse(e.ChosenClass);
				if (chosen != null)
				{
					if (chosen.ParentClass != null)
					{
						// Subclass selected. Replace parent class with subclass.
						CharacterClass parent = chosen.GetParent();

						if (parent != null)
						{
							pm.ReplaceSubclass(parent, chosen);
							pm.PendingLevels--;
							pm.SendMessage(0x35, "You are now a {0}.", chosen.Name);
						}
					}
					else
					{
						pm.AddClassLevel(chosen);
					}
				}
			}

			// Validate and apply Feats
			if (!string.IsNullOrEmpty(e.ChosenFeat))
			{
				Feat f = Feat.Parse(e.ChosenFeat);
				if (f != null && pm.PendingAbilityScorePoints >= 2) // Assume feat costs 1 ASI (2 points)
				{
					if (f.CanSelect(pm))
					{
						pm.Feats.Add(f);
						f.OnSelected(pm);
						pm.PendingAbilityScorePoints -= 2;
						pm.SendMessage("You have gained the {0} feat.", f.Name);
					}
				}
			}

			// Validate and apply ASI
			int asiSum = 0;
			for (int i = 0; i < 6; i++)
			{
				asiSum += e.AbilityIncreases[i];
			}

			if (asiSum > 0 && asiSum <= pm.PendingAbilityScorePoints)
			{
				AbilityScores scores = pm.AbilityScores;
				int newStr = scores.Str + e.AbilityIncreases[0];
				int newDex = scores.Dex + e.AbilityIncreases[1];
				int newCon = scores.Con + e.AbilityIncreases[2];
				int newInt = scores.Int + e.AbilityIncreases[3];
				int newWis = scores.Wis + e.AbilityIncreases[4];
				int newCha = scores.Cha + e.AbilityIncreases[5];

				if (newStr <= 20 && newDex <= 20 && newCon <= 20 && newInt <= 20 && newWis <= 20 && newCha <= 20)
				{
					pm.AbilityScores = new AbilityScores(newStr, newDex, newCon, newInt, newWis, newCha);
					pm.PendingAbilityScorePoints -= asiSum;
				}
			}

			// Validate and apply Spells
			if (e.SpellIds != null && e.SpellIds.Length > 0 && e.SpellIds.Length <= pm.PendingSpellsKnown)
			{
				int learned = 0;
				foreach (int spellId in e.SpellIds)
				{
					if (!pm.KnownSpells.Contains(spellId))
					{
						Server.Spells.DnD.DnDSpell spell = Server.Spells.DnD.SpellRegistry.FindById(spellId);
						if (spell != null)
						{
							pm.KnownSpells.Add(spellId);
							learned++;
						}
					}
				}
				pm.PendingSpellsKnown -= learned;
			}

			if (pm.NetState != null)
			{
				pm.NetState.Send(new DnDStatSync(pm));
				Server.Spells.DnD.DnDSpellPackets.SendSpellList(pm);
				if (pm.PendingLevels > 0 || pm.PendingAbilityScorePoints > 0 || pm.PendingSpellsKnown > 0)
				{
					Server.Spells.DnD.DnDSpellPackets.Send_DnDLevelUpPrompt(pm);
				}
			}
		}
	}
}
