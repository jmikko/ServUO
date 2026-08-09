using System;

namespace Server.Engines.Classes.Features
{
	/// <summary>
	/// The features that make each subclass different from its parent.
	/// <para>
	/// Eleven of the twelve subclasses had none - they resolved their parent correctly and were
	/// otherwise indistinguishable from it, because most of them needed systems that did not exist:
	/// Circle of the Land wanted Wild Shape, the Fiend wanted invocations, the Way of the Open Hand
	/// wanted ki. Those exist now, so these do too.
	/// </para>
	/// <para>
	/// Each subclass inherits its parent's whole feature list and adds these, so a Berserker is a
	/// Barbarian who also has Frenzy rather than a Barbarian who has lost Rage.
	/// </para>
	/// </summary>
	public sealed class FrenzyFeature : ClassFeature
	{
		public override string Name { get { return "Frenzy"; } }
		public override int Level { get { return 3; } }

		public override string Description
		{
			get { return "While raging, you may make a bonus attack each round - at the cost of exhaustion."; }
		}

		public override int GetUses(int classLevel) { return 1; }

		public override bool RecoversOnShortRest { get { return false; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (!ClassFeatures.ResistsPhysicalDamage(character))
			{
				user.SendMessage("You must be raging to enter a frenzy.");
				return false;
			}

			DnDRollModifiers.AddAdvantage(user, RollKind.Attack, TimeSpan.FromMinutes(1.0), Name);

			user.SendMessage(0x22, "You give yourself over to the frenzy.");

			return true;
		}
	}

	/// <summary>College of Lore: Cutting Words, which is a reaction that spoils someone else's roll.</summary>
	public sealed class CuttingWordsFeature : ClassFeature
	{
		public override string Name { get { return "Cutting Words"; } }
		public override int Level { get { return 3; } }

		public override string Description
		{
			get { return "As a reaction, you subtract a die from an enemy's roll."; }
		}

		public override int GetUses(int classLevel) { return 1; }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			Mobile target = user.Combatant as Mobile;

			if (target == null)
			{
				user.SendMessage("You have no one to belittle.");
				return false;
			}

			if (!DnDTurn.TrySpendReaction(user))
			{
				user.SendMessage("You have already used your reaction this round.");
				return false;
			}

			DnDRollModifiers.Add(
				target, Name, 6, 0, -1, RollKind.Attack | RollKind.AbilityCheck,
				TimeSpan.FromSeconds(12.0), true);

			user.SendMessage(0x35, "Your words find their mark; {0} falters.", target.Name);

			return true;
		}
	}

	/// <summary>Life Domain: Disciple of Life, which makes every healing spell go further.</summary>
	public sealed class DiscipleOfLifeFeature : ClassFeature
	{
		public override string Name { get { return "Disciple of Life"; } }
		public override int Level { get { return 1; } }

		public override string Description
		{
			get { return "Your healing spells restore extra hit points."; }
		}

		/// <summary>
		/// Read by the spell resolver rather than applied here, because it modifies a spell's result
		/// and a feature has no way to reach into one otherwise.
		/// </summary>
		public static int GetHealingBonus(IDnDCharacter character, int spellLevel)
		{
			if (!ClassFeatures.Has(character, "Disciple of Life"))
			{
				return 0;
			}

			return 2 + spellLevel;
		}
	}

	/// <summary>Circle of the Land: Natural Recovery, spell slots back on a short rest.</summary>
	public sealed class NaturalRecoveryFeature : ClassFeature
	{
		public override string Name { get { return "Natural Recovery"; } }
		public override int Level { get { return 2; } }

		public override string Description
		{
			get { return "Once per day, recover spell slots on a short rest."; }
		}

		public override int GetUses(int classLevel) { return 1; }

		public override bool RecoversOnShortRest { get { return false; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			var pm = user as Mobiles.DnDPlayerMobile;

			if (pm == null)
			{
				return false;
			}

			// Half the Druid level in slot levels, which is what the rules grant.
			int budget = Math.Max(1, (classLevel + 1) / 2);
			int recovered = 0;

			for (int level = Math.Min(5, budget); level >= 1 && budget > 0; --level)
			{
				while (budget >= level && pm.RestoreSpellSlot(level))
				{
					budget -= level;
					++recovered;
				}
			}

			if (recovered == 0)
			{
				user.SendMessage("You have no spent slots to recover.");
				return false;
			}

			user.SendMessage(0x35, "The land restores {0} spell slot(s) to you.", recovered);

			return true;
		}
	}

	/// <summary>Way of the Open Hand: Open Hand Technique, which rides on Flurry of Blows.</summary>
	public sealed class OpenHandTechniqueFeature : ClassFeature
	{
		public override string Name { get { return "Open Hand Technique"; } }
		public override int Level { get { return 3; } }

		public override string Description
		{
			get { return "Spend 1 ki to knock an enemy prone as you strike."; }
		}

		public override int GetUses(int classLevel) { return 0; }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			Mobile target = user.Combatant as Mobile;

			if (target == null)
			{
				user.SendMessage("You are not fighting anyone.");
				return false;
			}

			if (!DnDResourcePools.Spend(user, character, ResourcePoolType.Ki, 1))
			{
				user.SendMessage("You have no ki left.");
				return false;
			}

			int dc = Spellcasting.GetSaveDC(character);

			if (CombatRules.CheckSave(target, AbilityScoreType.Dex, dc))
			{
				user.SendMessage("{0} keeps their footing.", target.Name);
				return true;
			}

			DnDConditions.Add(target, DnDCondition.Prone, TimeSpan.FromSeconds(12.0));

			user.SendMessage(0x35, "You sweep {0} off their feet.", target.Name);

			return true;
		}
	}

	/// <summary>Oath of Devotion: Sacred Weapon, Charisma added to attacks for a minute.</summary>
	public sealed class SacredWeaponFeature : ClassFeature
	{
		public override string Name { get { return "Sacred Weapon"; } }
		public override int Level { get { return 3; } }

		public override string Description
		{
			get { return "Your weapon shines: add your Charisma modifier to attack rolls for a minute."; }
		}

		public override int GetUses(int classLevel) { return 1; }

		public override bool RecoversOnShortRest { get { return true; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			int bonus = Math.Max(1, character.EffectiveAbilityScores.ChaMod);

			DnDRollModifiers.Add(
				user, Name, 0, bonus, 1, RollKind.Attack, TimeSpan.FromMinutes(1.0), false);

			user.SendMessage(0x35, "Your weapon blazes with holy light.");

			return true;
		}
	}

	/// <summary>Hunter: Colossus Slayer, extra damage against a wounded foe.</summary>
	public sealed class ColossusSlayerFeature : ClassFeature
	{
		public override string Name { get { return "Colossus Slayer"; } }
		public override int Level { get { return 3; } }

		public override string Description
		{
			get { return "Once a round, you deal an extra 1d8 to a creature already wounded."; }
		}

		public override string GetBonusDamage(IDnDCharacter character, int classLevel, RollMode mode)
		{
			// Once per round, which the reaction budget is the wrong currency for - so it rides on
			// the bonus action, the only per-round resource a passive can reasonably claim.
			var m = character as Mobile;

			return m != null && DnDTurn.TrySpendBonusAction(m) ? "1d8" : null;
		}
	}

	/// <summary>Thief: Fast Hands, a second bonus action's worth of usefulness.</summary>
	public sealed class FastHandsFeature : ClassFeature
	{
		public override string Name { get { return "Fast Hands"; } }
		public override int Level { get { return 3; } }

		public override string Description
		{
			get { return "Your bonus action returns to you once per short rest."; }
		}

		public override int GetUses(int classLevel) { return 1; }

		public override bool RecoversOnShortRest { get { return true; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			DnDTurn.Reset(user);

			user.SendMessage(0x35, "Your hands move faster than thought.");

			return true;
		}
	}

	/// <summary>Draconic Bloodline: Draconic Resilience, tougher skin and more hit points.</summary>
	public sealed class DraconicResilienceFeature : ClassFeature
	{
		public override string Name { get { return "Draconic Resilience"; } }
		public override int Level { get { return 1; } }

		public override string Description
		{
			get { return "Your skin hardens: 13 + Dexterity armour class when unarmoured."; }
		}

		public override int GetUnarmoredArmorClass(IDnDCharacter character, int classLevel)
		{
			return 13 + character.EffectiveAbilityScores.DexMod;
		}
	}

	/// <summary>The Fiend: Dark One's Blessing, temporary vigour when an enemy falls.</summary>
	public sealed class DarkOnesBlessingFeature : ClassFeature
	{
		public override string Name { get { return "Dark One's Blessing"; } }
		public override int Level { get { return 1; } }

		public override string Description
		{
			get { return "When you drop a foe, you gain hit points equal to your Charisma and level."; }
		}

		/// <summary>Called from the combat resolver when something the Warlock was fighting dies.</summary>
		public static void OnKill(Mobile killer, IDnDCharacter character)
		{
			int classLevel;

			if (ClassFeatures.Find(character, "Dark One's Blessing", out classLevel) == null)
			{
				return;
			}

			int gained = Math.Max(1, character.EffectiveAbilityScores.ChaMod) + classLevel;

			killer.Hits = Math.Min(killer.HitsMax, killer.Hits + gained);

			killer.SendMessage(0x35, "Your patron rewards the kill: {0} hit points.", gained);
		}
	}

	/// <summary>School of Evocation: Potent Cantrip, a cantrip still bites on a successful save.</summary>
	public sealed class PotentCantripFeature : ClassFeature
	{
		public override string Name { get { return "Potent Cantrip"; } }
		public override int Level { get { return 6; } }

		public override string Description
		{
			get { return "Your cantrips deal half damage even when the target saves."; }
		}

		/// <summary>Read by the spell resolver, which is the only thing that knows a save was made.</summary>
		public static bool AppliesTo(IDnDCharacter character, int spellLevel)
		{
			return spellLevel == 0 && ClassFeatures.Has(character, "Potent Cantrip");
		}
	}
}
