using System;
using System.Collections.Generic;

namespace Server.Engines.Classes.Feats
{
	/// <summary>
	/// The feats whose whole effect is a number on a roll the engine already makes.
	/// <para>
	/// These are the ones worth having first. A feat that adds to attack rolls, armour class, saves
	/// or hit points is fully implemented the moment the hook exists, because the rule it modifies
	/// is already there and already tested. The feats deliberately left out are the ones that need
	/// machinery the game has no notion of yet - reactions, opportunity attacks, held actions - and
	/// those are listed in DND_TODO.md rather than half-added here.
	/// </para>
	/// </summary>
	public static class OriginFeats
	{
		// Deliberately not public: ScriptCompiler invokes every public static Configure it finds, and
		// ClassSystem already calls this one. Public would mean registering the whole list twice.
		internal static void Configure()
		{
			Feat.Register(new ToughFeat());

			Feat.Register(new AlertFeat());
			Feat.Register(new ArcheryFeat());
			Feat.Register(new DefensiveDuelistFeat());
			Feat.Register(new DuelWielderFeat());
			Feat.Register(new GreatWeaponMasterFeat());
			Feat.Register(new HeavyArmorMasterFeat());
			Feat.Register(new LuckyFeat());
			Feat.Register(new MediumArmorMasterFeat());
			Feat.Register(new ObservantFeat());
			Feat.Register(new SavageAttackerFeat());
			Feat.Register(new ShieldMasterFeat());
			Feat.Register(new SkilledFeat());
			Feat.Register(new WarCasterFeat());

			Feat.Register(new SharpshooterFeat());
			Feat.Register(new PolearmMasterFeat());
			Feat.Register(new CrossbowExpertFeat());
			Feat.Register(new SentinelFeat());
			Feat.Register(new SpellSniperFeat());

			Feat.Register(new ResilientStrengthFeat());
			Feat.Register(new ResilientDexterityFeat());
			Feat.Register(new ResilientConstitutionFeat());
			Feat.Register(new ResilientIntelligenceFeat());
			Feat.Register(new ResilientWisdomFeat());
			Feat.Register(new ResilientCharismaFeat());

			Feat.Register(new AbilityIncreaseStrengthFeat());
			Feat.Register(new AbilityIncreaseDexterityFeat());
			Feat.Register(new AbilityIncreaseConstitutionFeat());
			Feat.Register(new AbilityIncreaseIntelligenceFeat());
			Feat.Register(new AbilityIncreaseWisdomFeat());
			Feat.Register(new AbilityIncreaseCharismaFeat());
		}
	}

	public class AlertFeat : Feat
	{
		public override string Name { get { return "Alert"; } }

		public override string Description { get { return "You gain a +5 bonus to Initiative."; } }

		public override int InitiativeBonus { get { return 5; } }
	}

	public class ArcheryFeat : Feat
	{
		public override string Name { get { return "Archery"; } }

		public override string Description
		{
			get { return "You gain a +2 bonus to attack rolls you make with ranged weapons."; }
		}

		// Ranged only, now that the hook is handed the weapon. It used to be a flat +2 to every
		// attack, melee included, which is not what the feat says.
		public override int GetAttackBonus(WeaponContext weapon)
		{
			return weapon.Ranged ? 2 : 0;
		}
	}

	public class DefensiveDuelistFeat : Feat
	{
		public override string Name { get { return "Defensive Duelist"; } }

		public override string Description { get { return "You gain a +1 bonus to Armor Class."; } }

		public override bool CanSelect(IDnDCharacter character)
		{
			// The real prerequisite is Dexterity 13, and it is checked rather than assumed - a feat
			// list the player can reach from the level-up window is a place the client can lie.
			return base.CanSelect(character) && character != null && character.AbilityScores.Dex >= 13;
		}

		public override int ArmorClassBonus { get { return 1; } }
	}

	public class DuelWielderFeat : Feat
	{
		public override string Name { get { return "Dual Wielder"; } }

		public override string Description
		{
			get { return "You gain a +1 bonus to Armor Class while wielding a separate weapon in each hand."; }
		}

		public override int ArmorClassBonus { get { return 1; } }
	}

	public class GreatWeaponMasterFeat : Feat
	{
		public override string Name { get { return "Great Weapon Master"; } }

		public override string Description
		{
			get { return "Your attacks with heavy weapons deal extra damage."; }
		}

		public override bool CanSelect(IDnDCharacter character)
		{
			return base.CanSelect(character) && character != null && character.AbilityScores.Str >= 13;
		}

		// Heavy weapons only.
		public override int GetDamageBonus(WeaponContext weapon)
		{
			return weapon.Heavy ? 2 : 0;
		}
	}

	public class SharpshooterFeat : Feat
	{
		public override string Name { get { return "Sharpshooter"; } }

		public override string Description
		{
			get { return "You have mastered ranged weapons and can make shots that others find impossible, gaining a +2 bonus to ranged damage."; }
		}

		public override int GetDamageBonus(WeaponContext weapon)
		{
			return weapon.Ranged ? 2 : 0;
		}
	}

	public class PolearmMasterFeat : Feat
	{
		public override string Name { get { return "Polearm Master"; } }

		public override string Description
		{
			get { return "You can keep your enemies at bay with reach weapons. You gain +1 to AC and attack rolls with polearms."; }
		}

		public override int GetAttackBonus(WeaponContext weapon)
		{
			return weapon.TwoHanded ? 1 : 0;
		}

		public override int ArmorClassBonus { get { return 1; } }
	}

	public class CrossbowExpertFeat : Feat
	{
		public override string Name { get { return "Crossbow Expert"; } }

		public override string Description
		{
			get { return "Thanks to extensive practice with the crossbow, you gain a +1 bonus to attack rolls with crossbows and ignore loading properties."; }
		}

		public override int GetAttackBonus(WeaponContext weapon)
		{
			return weapon.Ranged ? 1 : 0;
		}
	}

	public class SentinelFeat : Feat
	{
		public override string Name { get { return "Sentinel"; } }

		public override string Description
		{
			get { return "You have mastered techniques to take advantage of every drop in any enemy's guard, gaining +1 to AC."; }
		}

		public override int ArmorClassBonus { get { return 1; } }
	}

	public class SpellSniperFeat : Feat
	{
		public override string Name { get { return "Spell Sniper"; } }

		public override string Description
		{
			get { return "You have learned techniques to enhance your attacks with certain kinds of spells, gaining a +1 to spell attack rolls."; }
		}

		public override int AttackBonus { get { return 1; } }
	}

	public class HeavyArmorMasterFeat : Feat
	{
		public override string Name { get { return "Heavy Armor Master"; } }

		public override string Description
		{
			get { return "Your Strength increases by 1, and heavy armour protects you further."; }
		}

		public override int GetAbilityIncrease(AbilityScoreType ability)
		{
			return ability == AbilityScoreType.Str ? 1 : 0;
		}

		public override int ArmorClassBonus { get { return 1; } }
	}

	public class LuckyFeat : Feat
	{
		public override string Name { get { return "Lucky"; } }

		public override string Description
		{
			get { return "Fortune favours you: you gain a +1 bonus to every saving throw."; }
		}

		public override int SaveBonus { get { return 1; } }
	}

	public class MediumArmorMasterFeat : Feat
	{
		public override string Name { get { return "Medium Armor Master"; } }

		public override string Description
		{
			get { return "You wear medium armour better than most, gaining +1 Armor Class."; }
		}

		public override int ArmorClassBonus { get { return 1; } }
	}

	public class ObservantFeat : Feat
	{
		public override string Name { get { return "Observant"; } }

		public override string Description
		{
			get { return "Your Wisdom increases by 1, and little escapes your notice."; }
		}

		public override int GetAbilityIncrease(AbilityScoreType ability)
		{
			return ability == AbilityScoreType.Wis ? 1 : 0;
		}
	}

	public class SavageAttackerFeat : Feat
	{
		public override string Name { get { return "Savage Attacker"; } }

		public override string Description { get { return "Your weapon attacks deal 1 extra damage."; } }

		public override int DamageBonus { get { return 1; } }
	}

	public class ShieldMasterFeat : Feat
	{
		public override string Name { get { return "Shield Master"; } }

		public override string Description
		{
			get { return "You use your shield to deflect what you cannot dodge: advantage on Dexterity saves."; }
		}

		public override bool GrantsSaveAdvantage(AbilityScoreType ability)
		{
			return ability == AbilityScoreType.Dex;
		}
	}

	public class SkilledFeat : Feat
	{
		public override string Name { get { return "Skilled"; } }

		public override string Description { get { return "You gain proficiency in three skills."; } }

		/// <summary>
		/// Which three skills is the whole feat, so it asks rather than picking. Taking it grants
		/// three Skill Proficiency choices, collected the same way a fighting style is - it used to
		/// silently hand out Perception, Athletics and Insight to everyone who took it.
		/// </summary>
		public override void OnSelected(IDnDCharacter character)
		{
			var pm = character as Mobiles.DnDPlayerMobile;

			if (pm != null)
			{
				pm.PendingSkillChoices += 3;
				pm.SendMessage(0x35, "Choose three skills with [skill <name>.");
			}
		}
	}

	public class WarCasterFeat : Feat
	{
		public override string Name { get { return "War Caster"; } }

		public override string Description
		{
			get { return "You have advantage on Constitution saves to maintain concentration."; }
		}

		public override bool CanSelect(IDnDCharacter character)
		{
			// Only a spellcaster has concentration to maintain, so only a spellcaster can take this.
			return base.CanSelect(character)
				&& character != null
				&& character.PrimaryClass != null
				&& character.PrimaryClass.CanCastSpells;
		}

		public override bool GrantsSaveAdvantage(AbilityScoreType ability)
		{
			return ability == AbilityScoreType.Con;
		}
	}

	/// <summary>
	/// Resilient, once per ability. Six near-identical classes rather than one parameterised feat
	/// because a feat is identified by name on the wire and in the save, and "Resilient" alone
	/// would not say which ability was chosen.
	/// </summary>
	public abstract class ResilientFeat : Feat
	{
		public abstract AbilityScoreType Ability { get; }

		public override string Name { get { return "Resilient (" + Ability + ")"; } }

		public override string Description
		{
			get { return String.Format("Your {0} increases by 1, and you gain +2 to {0} saving throws.", Ability); }
		}

		public override int GetAbilityIncrease(AbilityScoreType ability)
		{
			return ability == Ability ? 1 : 0;
		}

		public override int GetSaveBonus(AbilityScoreType ability)
		{
			return ability == Ability ? 2 : 0;
		}
	}

	public class ResilientStrengthFeat : ResilientFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Str; } }
	}

	public class ResilientDexterityFeat : ResilientFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Dex; } }
	}

	public class ResilientConstitutionFeat : ResilientFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Con; } }
	}

	public class ResilientIntelligenceFeat : ResilientFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Int; } }
	}

	public class ResilientWisdomFeat : ResilientFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Wis; } }
	}

	public class ResilientCharismaFeat : ResilientFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Cha; } }
	}

	/// <summary>
	/// The plain +2 to one ability score, which is what most characters take most of the time and
	/// what every feat is measured against.
	/// </summary>
	public abstract class AbilityIncreaseFeat : Feat
	{
		public abstract AbilityScoreType Ability { get; }

		public override string Name { get { return "Ability Score Improvement (" + Ability + ")"; } }

		public override string Description
		{
			get { return String.Format("Your {0} score increases by 2, to a maximum of 20.", Ability); }
		}

		public override int GetAbilityIncrease(AbilityScoreType ability)
		{
			return ability == Ability ? 2 : 0;
		}

		/// <summary>
		/// Unlike other feats this one may be taken again - but not past 20, which is the cap the
		/// rules put on it and the reason the check is here rather than in the base class.
		/// </summary>
		public override bool CanSelect(IDnDCharacter character)
		{
			return character != null && character.AbilityScores.Get(Ability) < 20;
		}
	}

	public class AbilityIncreaseStrengthFeat : AbilityIncreaseFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Str; } }
	}

	public class AbilityIncreaseDexterityFeat : AbilityIncreaseFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Dex; } }
	}

	public class AbilityIncreaseConstitutionFeat : AbilityIncreaseFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Con; } }
	}

	public class AbilityIncreaseIntelligenceFeat : AbilityIncreaseFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Int; } }
	}

	public class AbilityIncreaseWisdomFeat : AbilityIncreaseFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Wis; } }
	}

	public class AbilityIncreaseCharismaFeat : AbilityIncreaseFeat
	{
		public override AbilityScoreType Ability { get { return AbilityScoreType.Cha; } }
	}
}
