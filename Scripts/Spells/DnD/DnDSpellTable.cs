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

		/// <summary>Does nothing mechanical yet - flavour, light, and the like.</summary>
		Utility
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
		public AbilityScoreType SaveAbility;
		public bool HalfOnSave;
		public int Range;
		public bool Beneficial;
		public bool SelfOnly;

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

		public int AreaRadius;
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
				SaveAbility = ParseEnum(el.GetAttribute("save"), AbilityScoreType.Dex),
				HalfOnSave = el.GetAttribute("halfOnSave") == "true",
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
				AreaRadius = ParseInt(el.GetAttribute("radius")),
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
		public override AbilityScoreType SaveAbility { get { return m_Data.SaveAbility; } }
		public override int Range { get { return m_Data.Range; } }
		public override bool HalfDamageOnSave { get { return m_Data.HalfOnSave; } }
		public override bool Beneficial { get { return m_Data.Beneficial; } }
		public override int AreaRadius { get { return m_Data.AreaRadius; } }
		public override bool RequiresConcentration { get { return m_Data.Concentration; } }
		public override TimeSpan Duration { get { return m_Data.Duration; } }

		public override void Effect(Mobile caster, IDnDCharacter character, Mobile target, int slotLevel)
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
				case SpellEffectKind.Utility:
					{
						caster.SendMessage("{0} takes effect.", Name);
						break;
					}
			}
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
		/// Cantrips add dice at 5th, 11th and 17th character level; levelled spells add dice per
		/// slot level spent above their own.
		/// </summary>
		private int RollDice(IDnDCharacter character, int slotLevel)
		{
			int dice = m_Data.DiceCount;

			if (m_Data.ScalesWithCantripDice)
			{
				dice *= Spellcasting.GetCantripDice(character.CharacterLevel);
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
