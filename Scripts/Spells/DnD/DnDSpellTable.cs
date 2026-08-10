using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Server.Spells.DnD
{
	/// <summary>
	/// What a data-driven spell does when it lands. Anything that needs behaviour beyond these
	/// stays a hand-written <see cref="DnDSpell"/> subclass.
	/// </summary>
	public enum SpellEffectKind
	{
		/// <summary>Rolls dice and applies them as damage.</summary>
		Damage,

		/// <summary>Rolls dice and restores that many hit points.</summary>
		Healing,

		/// <summary>Sets the target's armour class to a floor for a duration.</summary>
		ArmorClass,

		/// <summary>Inflicts a condition, subject to a saving throw where the spell allows one.</summary>
		Condition,

		/// <summary>Adds or subtracts a die on future rolls - the Bless and Bane family.</summary>
		RollModifier,

		/// <summary>Does nothing mechanical yet - flavour, light, and the like.</summary>
		Utility,

		/// <summary>Teleports the caster to a location.</summary>
		Teleport,

		/// <summary>Grants a movement mode (like flying) for a duration.</summary>
		MovementMode,

		/// <summary>
		/// Brings someone back - from dying, or from dead. Revivify and the raise-dead family.
		/// <para>
		/// These were flavour text until death saving throws existed, because there was nothing
		/// between "alive" and "a ghost looking for a healer" for a spell to reach into. Now there
		/// is, and the whole point of the three rounds is that someone might spend one of them
		/// casting this.
		/// </para>
		/// </summary>
		Revive,

		/// <summary>Removes conditions - the restoration family, and Remove Curse.</summary>
		RemoveCondition,

		/// <summary>Halves incoming physical damage for a duration.</summary>
		Resistance,

		/// <summary>Grants advantage on a kind of roll for a duration.</summary>
		Advantage,

		/// <summary>Ends magical effects on the target - Dispel Magic and Counterspell.</summary>
		Dispel,

		/// <summary>Sheds light, or grants the ability to see without it.</summary>
		Light
	}

	/// <summary>
	/// One spell as a row of data. The great majority of SRD spells are "roll N dice of this size,
	/// resolved this way, scaling like this" - writing a class each for those buys nothing, so they
	/// live in Data/DnDSpells.xml and are instantiated as <see cref="DataDrivenSpell"/>.
	/// </summary>
	public sealed class DnDSpellData
	{
		public string Id;
		public string Name;
		public int Level;
		public SpellSchool School;
		public SpellEffectKind Kind;
		public SpellResolution Resolution;

		/// <summary>What the damage is made of, for the target.s resistances. None if it deals none.</summary>
		public DnDDamageType DamageType;
		public AbilityScoreType SaveAbility;
		public bool HalfOnSave;
		public int Range;
		public bool Beneficial;
		public bool SelfOnly;
		public SpellTargetType TargetType;

		/// <summary>Dice per "step" - one step for a cantrip at 1st level, or one slot level.</summary>
		public int DiceCount;

		public int DiceSides;

		/// <summary>Flat bonus inside the damage roll, before any ability modifier.</summary>
		public int DiceBonus;

		/// <summary>Adds the caster's spellcasting modifier to the result (healing usually does).</summary>
		public bool AddCastingModifier;

		/// <summary>Extra dice per slot level above the spell's own level.</summary>
		public int DicePerSlotLevel;

		/// <summary>Cantrips scale on character level rather than slots.</summary>
		public bool ScalesWithCantripDice;

		/// <summary>For ArmorClass effects: the AC floor this spell sets.</summary>
		public int ArmorClassValue;

		public TimeSpan Duration;

		/// <summary>For Condition effects: what the spell inflicts.</summary>
		public DnDCondition Condition;

		/// <summary>
		/// For Condition effects: the highest hit point total a creature can have and still be
		/// affected. 0 means no limit. This is how Sleep works - it simply overwhelms the weak.
		/// </summary>
		public int HitPointThreshold;

		/// <summary>For RollModifier effects: the die added, which rolls it applies to, and its sign.</summary>
		public int ModifierDie;
		
		public int ModifierBonus;

		public RollKind ModifierKinds;
		public bool ModifierIsPenalty;

		/// <summary>Guidance and Resistance are spent by the first roll that uses them.</summary>
		public bool ModifierOneShot;

		public SpellShape Shape;

		/// <summary>Tiles: a sphere's radius, a cone or line's length, a cube's half-extent.</summary>
		public int AreaSize;

		public bool Concentration;

		public string Description;

		public string[] Classes = new string[0];
	}

	/// <summary>Loads Data/DnDSpells.xml and registers every row with <see cref="SpellRegistry"/>.</summary>
	public static class DnDSpellTable
	{
		public static void Configure()
		{
			string path = Path.Combine(Core.BaseDirectory, "Data", "DnDSpells.xml");

			if (!File.Exists(path))
			{
				Console.WriteLine("Warning: {0} does not exist, no spells loaded", path);
				return;
			}

			var doc = new XmlDocument();
			doc.Load(path);

			int count = 0;

			foreach (XmlElement el in doc.SelectNodes("//spell"))
			{
				DnDSpellData data = Parse(el);

				SpellRegistry.Register(new DataDrivenSpell(data), data.Classes);
				++count;
			}

			Console.WriteLine("Spells: loaded {0} spell(s) from Data/DnDSpells.xml", count);
		}

		private static DnDSpellData Parse(XmlElement el)
		{
			var data = new DnDSpellData
			{
				Id = el.GetAttribute("id"),
				Name = el.GetAttribute("name"),
				Level = ParseInt(el.GetAttribute("level")),
				School = ParseEnum(el.GetAttribute("school"), SpellSchool.Evocation),
				Kind = ParseEnum(el.GetAttribute("kind"), SpellEffectKind.Damage),
				Resolution = ParseEnum(el.GetAttribute("resolution"), SpellResolution.Automatic),
				DamageType = DnDMonsterTraits.ParseDamageTypes(
					el.GetAttribute("damageType"), el.GetAttribute("id"), "damageType"),
				SaveAbility = ParseEnum(el.GetAttribute("save"), AbilityScoreType.Dex),
				HalfOnSave = el.GetAttribute("halfOnSave") == "true",
				TargetType = ParseEnum(el.GetAttribute("targetType"), SpellTargetType.Mobile),
				Range = ParseInt(el.GetAttribute("range"), 12),
				Beneficial = el.GetAttribute("beneficial") == "true",
				SelfOnly = el.GetAttribute("selfOnly") == "true",
				DiceCount = ParseInt(el.GetAttribute("dice")),
				DiceSides = ParseInt(el.GetAttribute("die")),
				DiceBonus = ParseInt(el.GetAttribute("bonus")),
				AddCastingModifier = el.GetAttribute("addModifier") == "true",
				DicePerSlotLevel = ParseInt(el.GetAttribute("dicePerSlot")),
				ScalesWithCantripDice = el.GetAttribute("cantripScaling") == "true",
				ArmorClassValue = ParseInt(el.GetAttribute("armorClass")),
				Condition = ParseEnum(el.GetAttribute("condition"), DnDCondition.None),
				HitPointThreshold = ParseInt(el.GetAttribute("hitPointThreshold")),
				ModifierDie = ParseInt(el.GetAttribute("modifierDie")),
				ModifierBonus = ParseInt(el.GetAttribute("modifierBonus")),
				ModifierKinds = ParseModifierKinds(el.GetAttribute("modifierKinds")),
				ModifierIsPenalty = el.GetAttribute("modifierPenalty") == "true",
				ModifierOneShot = el.GetAttribute("modifierOneShot") == "true",
				Shape = ParseEnum(el.GetAttribute("shape"), SpellShape.Single),
				AreaSize = ParseInt(el.GetAttribute("size")),
				Concentration = el.GetAttribute("concentration") == "true",
				Description = el.GetAttribute("description")
			};

			int seconds = ParseInt(el.GetAttribute("durationSeconds"));

			data.Duration = TimeSpan.FromSeconds(seconds);

			string classes = el.GetAttribute("classes");

			if (!String.IsNullOrEmpty(classes))
			{
				string[] parts = classes.Split(',');

				for (int i = 0; i < parts.Length; ++i)
				{
					parts[i] = parts[i].Trim();
				}

				data.Classes = parts;
			}

			return data;
		}

		private static RollKind ParseModifierKinds(string value)
		{
			RollKind result = RollKind.None;

			if (String.IsNullOrEmpty(value))
			{
				return result;
			}

			foreach (string part in value.Split(','))
			{
				result |= ParseEnum(part.Trim(), RollKind.None);
			}

			return result;
		}

		private static T ParseEnum<T>(string value, T fallback) where T : struct
		{
			T result;

			return !String.IsNullOrEmpty(value) && Enum.TryParse(value, true, out result) ? result : fallback;
		}

		private static int ParseInt(string value, int fallback = 0)
		{
			int result;

			return !String.IsNullOrEmpty(value) &&
				   Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result)
				? result
				: fallback;
		}
	}

	/// <summary>A spell whose whole behaviour is described by its <see cref="DnDSpellData"/> row.</summary>
	public sealed class DataDrivenSpell : DnDSpell
	{
		private readonly DnDSpellData m_Data;

		public DnDSpellData Data { get { return m_Data; } }

		public DataDrivenSpell(DnDSpellData data)
		{
			m_Data = data;
		}

		public override string Name { get { return m_Data.Name; } }
		public override int Level { get { return m_Data.Level; } }
		public override SpellSchool School { get { return m_Data.School; } }
		public override SpellResolution Resolution { get { return m_Data.Resolution; } }
		public override DnDDamageType DamageType { get { return m_Data.DamageType; } }
		public override AbilityScoreType SaveAbility { get { return m_Data.SaveAbility; } }
		public override int Range { get { return m_Data.Range; } }
		public override bool HalfDamageOnSave { get { return m_Data.HalfOnSave; } }
		public override SpellTargetType TargetType { get { return m_Data.TargetType; } }
		public override bool Beneficial { get { return m_Data.Beneficial; } }
		public override SpellShape Shape { get { return m_Data.Shape; } }
		public override int AreaSize { get { return m_Data.AreaSize; } }
		public override bool RequiresConcentration { get { return m_Data.Concentration; } }
		public override TimeSpan Duration { get { return m_Data.Duration; } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, Point3D targetLocation, int slotLevel)
		{
			switch (m_Data.Kind)
			{
				case SpellEffectKind.Damage:
					{
						DnDCasting.ApplySpellDamage(caster, character, target, this, RollDice(character, slotLevel));
						break;
					}
				case SpellEffectKind.Healing:
					{
						int healed = RollDice(character, slotLevel);

						if (healed > 0 && target.Alive)
						{
							target.Hits += healed;

							// Healing someone who is dying brings them round, which is the whole
							// point of the rounds they get before the saves run out.
							Mobiles.DnDDeath.OnHealed(target);
						}

						break;
					}
				case SpellEffectKind.ArmorClass:
					{
						DnDEffects.ApplyArmorClass(target, m_Data.ArmorClassValue, m_Data.Duration, Name);
						break;
					}
				case SpellEffectKind.Condition:
					{
						ApplyCondition(caster, character, target);
						break;
					}
				case SpellEffectKind.RollModifier:
					{
						ApplyRollModifier(caster, character, target);
						break;
					}
				case SpellEffectKind.Utility:
					{
						caster.SendMessage("{0} takes effect.", Name);
						break;
					}
				case SpellEffectKind.Teleport:
					{
						caster.MoveToWorld(targetLocation, caster.Map);
						caster.PlaySound(0x1FE); // Teleport sound
						break;
					}
				case SpellEffectKind.MovementMode:
					{
						DnDEffects.ApplyFlying(target, m_Data.Duration, Name);
						break;
					}
				case SpellEffectKind.Revive:
					{
						Revive(caster, character, target, slotLevel);
						break;
					}
				case SpellEffectKind.RemoveCondition:
					{
						RemoveConditions(caster, target);
						break;
					}
				case SpellEffectKind.Resistance:
					{
						Server.DnDRollModifiers.AddResistance(target, m_Data.Duration, Name);
						break;
					}
				case SpellEffectKind.Advantage:
					{
						Server.DnDRollModifiers.AddAdvantage(target, m_Data.ModifierKinds, m_Data.Duration, Name);
						break;
					}
				case SpellEffectKind.Dispel:
					{
						DnDEffects.Clear(target);
						DnDRollModifiers.Clear(target);
						DnDConcentration.End(target);

						caster.SendMessage("The magic on {0} unravels.", target.Name);
						break;
					}
				case SpellEffectKind.Light:
					{
						DnDEffects.ApplyLight(target, m_Data.Duration, Name);
						break;
					}
			}
		}

		/// <summary>
		/// Brings someone back. What "back" means depends on how far gone they are, which is the
		/// distinction death saving throws introduced: a dying character is picked up where they
		/// fell, a dead one needs the higher-level spells.
		/// </summary>
		private void Revive(Mobile caster, IDnDCharacter character, Mobile target, int slotLevel)
		{
			if (target == null)
			{
				return;
			}

			if (Mobiles.DnDDeath.IsDying(target))
			{
				int healed = Math.Max(1, RollDice(character, slotLevel));

				target.Hits = healed;

				Mobiles.DnDDeath.OnHealed(target);

				caster.SendMessage("{0} draws breath again.", target.Name);
				return;
			}

			if (!target.Alive)
			{
				target.Resurrect();

				Mobiles.DnDDeath.Clear(target);

				// The raise-dead family all return you at a single hit point - the spell buys you
				// your life back and nothing else, which is what makes it frightening to need one.
				// True Resurrection is the exception the SRD carves out, and the only reason a 9th
				// level slot is worth spending over a 5th.
				target.Hits = m_Data.Level >= 9 ? target.HitsMax : Math.Max(1, RollDice(character, slotLevel));

				caster.SendMessage("{0} returns from death.", target.Name);
				return;
			}

			caster.SendMessage("{0} is in no need of that.", target.Name);
		}

		/// <summary>
		/// Removes conditions. Which ones is the difference between Lesser and Greater Restoration,
		/// so the spell's own list is used rather than clearing everything.
		/// </summary>
		private void RemoveConditions(Mobile caster, Mobile target)
		{
			if (target == null)
			{
				return;
			}

			if (m_Data.Condition == DnDCondition.None)
			{
				DnDConditions.Clear(target);
				caster.SendMessage("{0} is freed of all that afflicted them.", target.Name);
				return;
			}

			DnDConditions.Remove(target, m_Data.Condition);

			caster.SendMessage("{0} is freed of {1}.", target.Name, m_Data.Condition);
		}

		/// <summary>
		/// Inflicts the spell's condition. A hit point threshold (Sleep) overrides the saving throw
		/// entirely - the spell simply overwhelms creatures below it and cannot touch those above.
		/// </summary>
		private void ApplyCondition(Mobile caster, IDnDCharacter character, Mobile target)
		{
			if (m_Data.HitPointThreshold > 0)
			{
				if (target.Hits > m_Data.HitPointThreshold)
				{
					caster.SendMessage("{0} is too strong to be affected.", target.Name);
					return;
				}
			}
			else if (m_Data.Resolution == SpellResolution.SavingThrow &&
					 CombatRules.CheckSave(target, m_Data.SaveAbility, Spellcasting.GetSaveDC(character)))
			{
				caster.SendMessage("{0} resists.", target.Name);
				return;
			}

			DnDConditions.Add(target, m_Data.Condition, m_Data.Duration);

			caster.SendMessage("{0} is {1}.", target.Name, m_Data.Condition.ToString().ToLowerInvariant());
		}

		/// <summary>
		/// Hangs a die on the target's future rolls. A penalty like Bane allows a save to avoid it;
		/// a blessing does not, since nobody resists being helped.
		/// </summary>
		private void ApplyRollModifier(Mobile caster, IDnDCharacter character, Mobile target)
		{
			if (m_Data.Resolution == SpellResolution.SavingThrow &&
				CombatRules.CheckSave(target, m_Data.SaveAbility, Spellcasting.GetSaveDC(character)))
			{
				caster.SendMessage("{0} resists.", target.Name);
				return;
			}

			Server.DnDRollModifiers.Add(
				target,
				Name,
				m_Data.ModifierDie,
				m_Data.ModifierBonus,
				m_Data.ModifierIsPenalty ? -1 : 1,
				m_Data.ModifierKinds,
				m_Data.Duration,
				m_Data.ModifierOneShot);

			target.SendMessage(
				"{0} {1} you {2}d{3} on {4}.",
				Name,
				m_Data.ModifierIsPenalty ? "costs" : "grants",
				1,
				m_Data.ModifierDie,
				DescribeKinds(m_Data.ModifierKinds));
		}

		private static string DescribeKinds(RollKind kinds)
		{
			var parts = new List<string>();

			if ((kinds & RollKind.Attack) != 0) { parts.Add("attack rolls"); }
			if ((kinds & RollKind.Save) != 0) { parts.Add("saving throws"); }
			if ((kinds & RollKind.AbilityCheck) != 0) { parts.Add("ability checks"); }

			return parts.Count == 0 ? "nothing" : String.Join(" and ", parts);
		}

		/// <summary>
		/// Cantrips add dice at 5th, 11th and 17th character level; levelled spells add dice per
		/// slot level spent above their own.
		/// </summary>
		private int RollDice(IDnDCharacter character, int slotLevel)
		{
			int dice = m_Data.DiceCount;

			if (m_Data.ScalesWithCantripDice)
			{
				dice *= Spellcasting.GetCantripDice(character.TotalLevel);
			}
			else if (m_Data.DicePerSlotLevel > 0)
			{
				dice += m_Data.DicePerSlotLevel * (slotLevel - m_Data.Level);
			}

			int total = Utility.Dice(dice, m_Data.DiceSides, m_Data.DiceBonus * dice);

			if (m_Data.AddCastingModifier)
			{
				total += Spellcasting.GetCastingAbilityModifier(character);
			}

			return total;
		}
	}
}
