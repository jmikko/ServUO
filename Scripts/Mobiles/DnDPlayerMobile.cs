using System;
using System.Collections.Generic;
using Server.Network;
using Server.Engines.Classes.Feats;

namespace Server.Mobiles
{
	public class DnDPlayerMobile : Mobile, IDnDCharacter
	{
		private bool m_DnDInitialized;
		private AbilityScores m_AbilityScores;
		private Dictionary<CharacterClass, int> m_Classes = new Dictionary<CharacterClass, int>();
		private CharacterClass m_PrimaryClass;
		private List<Feat> m_Feats = new List<Feat>();
		private List<DnDSkill> m_SkillProficiencies = new List<DnDSkill>();

		private int m_PendingAbilityScorePoints;
		private int m_PendingSpellsKnown;
		private int m_PendingLevels;
		private List<int> m_KnownSpells = new List<int>();

		private List<Item> m_AttunedItems = new List<Item>();

		[CommandProperty(AccessLevel.GameMaster)]
		public int PendingAbilityScorePoints
		{
			get { return m_PendingAbilityScorePoints; }
			set { m_PendingAbilityScorePoints = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int PendingSpellsKnown
		{
			get { return m_PendingSpellsKnown; }
			set { m_PendingSpellsKnown = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int PendingLevels
		{
			get { return m_PendingLevels; }
			set { m_PendingLevels = value; }
		}

		public List<int> KnownSpells { get { return m_KnownSpells; } }

		public List<Item> AttunedItems { get { return m_AttunedItems; } }

		public IReadOnlyDictionary<CharacterClass, int> Classes { get { return m_Classes; } }

		public List<Feat> Feats { get { return m_Feats; } }

		public bool IsAttunedTo(Item item)
		{
			return m_AttunedItems.Contains(item);
		}

		public bool IsProficient(DnDSkill skill)
		{
			return m_SkillProficiencies.Contains(skill);
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool DnDInitialized { get { return m_DnDInitialized; } }

		public AbilityScores AbilityScores { get { return m_AbilityScores; } set { m_AbilityScores = value; } }

		public AbilityScores EffectiveAbilityScores
		{
			get
			{

				int str = m_AbilityScores.Str;
				int dex = m_AbilityScores.Dex;
				int con = m_AbilityScores.Con;
				int @int = m_AbilityScores.Int;
				int wis = m_AbilityScores.Wis;
				int cha = m_AbilityScores.Cha;

				foreach (Item item in m_AttunedItems)
				{
					if (item != null && !item.Deleted && item is IDnDMagicItem magicItem)
					{
						int strOverride = magicItem.GetAbilityScoreOverride(AbilityScoreType.Str);
						if (strOverride > str) str = strOverride;

						int dexOverride = magicItem.GetAbilityScoreOverride(AbilityScoreType.Dex);
						if (dexOverride > dex) dex = dexOverride;

						int conOverride = magicItem.GetAbilityScoreOverride(AbilityScoreType.Con);
						if (conOverride > con) con = conOverride;

						int intOverride = magicItem.GetAbilityScoreOverride(AbilityScoreType.Int);
						if (intOverride > @int) @int = intOverride;

						int wisOverride = magicItem.GetAbilityScoreOverride(AbilityScoreType.Wis);
						if (wisOverride > wis) wis = wisOverride;

						int chaOverride = magicItem.GetAbilityScoreOverride(AbilityScoreType.Cha);
						if (chaOverride > cha) cha = chaOverride;
					}
				}

				return new AbilityScores(str, dex, con, @int, wis, cha);
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public CharacterClass PrimaryClass { get { return m_PrimaryClass; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int TotalLevel
		{
			get
			{
				int total = 0;
				foreach (var kv in m_Classes) total += kv.Value;
				return total;
			}
		}

		public DnDPlayerMobile()
		{
		}

		public DnDPlayerMobile(Serial serial)
			: base(serial)
		{
		}

		public override bool NewGuildDisplay { get { return true; } }

		#region Account bookkeeping
		public DateTime SessionStart { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan GameTime { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Young { get { return false; } set { } }
		#endregion

		[CommandProperty(AccessLevel.GameMaster)]
		public int ArmorClass
		{
			get
			{
				int baseAC = 10;
				int shieldBonus = 0;
				int maxDex = int.MaxValue;
				int magicBonus = 0;

				foreach (Item item in Items)
				{
					if (item is IDnDMagicItem magicItem)
					{
						if (!magicItem.RequiresAttunement || m_AttunedItems.Contains(item))
						{
							magicBonus += magicItem.ArmorClassBonus;
						}
					}

					IDnDEquipment eq = item as IDnDEquipment;

					if (eq == null || eq.ArmorCategory == ArmorCategory.None)
					{
						continue;
					}

					if (eq.ArmorCategory == ArmorCategory.Shield)
					{
						shieldBonus += eq.ArmorBonus;
						continue;
					}

					baseAC = eq.ArmorBonus;

					Items.DnDArmor armor = item as Items.DnDArmor;

					if (armor != null && armor.MaxDexBonus >= 0)
					{
						maxDex = armor.MaxDexBonus;
					}
				}

				AbilityScores effectiveStats = EffectiveAbilityScores;
				int dexMod = Math.Min(effectiveStats.DexMod, maxDex);

				int floor = Spells.DnD.DnDEffects.GetArmorClassFloor(this);

				return Math.Max(baseAC + dexMod, floor + (floor > 0 ? dexMod : 0)) + shieldBonus + magicBonus;
			}
		}

		public override int HitsMax
		{
			get
			{
				if (!m_DnDInitialized || m_PrimaryClass == null || m_Classes.Count == 0)
				{
					return 10;
				}

				int totalHitPoints = 0;
				bool firstClass = true;
				int conMod = EffectiveAbilityScores.ConMod;

				foreach (var kv in m_Classes)
				{
					int hitDie = kv.Key.HitDie;
					int level = kv.Value;
					int perLevelAverage = (hitDie / 2) + 1;

					if (firstClass)
					{
						totalHitPoints += hitDie + conMod + ((level - 1) * (perLevelAverage + conMod));
						firstClass = false;
					}
					else
					{
						totalHitPoints += level * (perLevelAverage + conMod);
					}
				}

				totalHitPoints = Math.Max(TotalLevel, totalHitPoints); // Never below 1 HP per level

				foreach (var feat in m_Feats)
				{
					if (feat is ToughFeat)
					{
						totalHitPoints += (TotalLevel * 2);
					}
				}

				return totalHitPoints;
			}
		}

		#region Advancement

		private int m_Experience;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Experience { get { return m_Experience; } }

		public void AwardExperience(int amount)
		{
			if (!m_DnDInitialized || amount <= 0 || TotalLevel + m_PendingLevels >= Advancement.MaxLevel)
			{
				return;
			}

			m_Experience += amount;

			SendMessage("You gain {0} experience.", amount);

			int newLevel = Advancement.GetLevelForExperience(m_Experience);
			int currentEffectiveLevel = TotalLevel + m_PendingLevels;

			if (newLevel > currentEffectiveLevel)
			{
				m_PendingLevels += (newLevel - currentEffectiveLevel);
				SendMessage(0x35, "You have gained enough experience to reach level {0}!", newLevel);
				Spells.DnD.DnDSpellPackets.Send_DnDLevelUpPrompt(this);
			}
		}

		public void AddClassLevel(CharacterClass c)
		{
			if (m_PendingLevels <= 0) return;

			int before = Hits;
			m_PendingLevels--;

			if (!m_Classes.ContainsKey(c)) m_Classes[c] = 0;
			m_Classes[c]++;

			if (m_PrimaryClass == null)
			{
				m_PrimaryClass = c;
			}

			Hits = Math.Min(HitsMax, before + GainedHitPointsThisLevel(c));

			int currentClassLevel = m_Classes[c];
			m_PendingAbilityScorePoints += c.GetAbilityScoreImprovements(currentClassLevel);

			int spellsBefore = c.GetSpellsKnown(currentClassLevel - 1);
			int spellsNow = c.GetSpellsKnown(currentClassLevel);

			if (spellsNow > spellsBefore && spellsNow != int.MaxValue)
			{
				m_PendingSpellsKnown += (spellsNow - spellsBefore);
			}

			SendMessage(0x35, "You are now a level {0} {1}.", currentClassLevel, c.Name);

			Delta(MobileDelta.Hits);

			if (NetState != null)
			{
				NetState.Send(new DnDStatSync(this));
			}

			Spells.DnD.DnDSpellPackets.SendSpellList(this);
		}

		public void ReplaceSubclass(CharacterClass parent, CharacterClass subclass)
		{
			if (m_Classes.ContainsKey(parent))
			{
				int level = m_Classes[parent];
				m_Classes.Remove(parent);
				m_Classes[subclass] = level;
				if (m_PrimaryClass == parent) m_PrimaryClass = subclass;
			}
		}

		private int GainedHitPointsThisLevel(CharacterClass c)
		{
			int hitDie = c.HitDie;
			int perLevelAverage = (hitDie / 2) + 1;
			int gained = perLevelAverage + EffectiveAbilityScores.ConMod;
			return Math.Max(1, gained);
		}

		#endregion

		public void ApplyDnDSetup(AbilityScores scores, CharacterClass characterClass)
		{
			if (m_DnDInitialized)
			{
				return;
			}

			m_AbilityScores = scores;
			m_PrimaryClass = characterClass;
			m_Classes[characterClass] = 1;
			m_DnDInitialized = true;

			m_SkillProficiencies.Clear();
			if (characterClass.Name == "Wizard" || characterClass.Name == "Sorcerer")
			{
				m_SkillProficiencies.Add(DnDSkill.Arcana);
				m_SkillProficiencies.Add(DnDSkill.History);
			}
			else if (characterClass.Name == "Rogue" || characterClass.Name == "Ranger" || characterClass.Name == "Monk")
			{
				m_SkillProficiencies.Add(DnDSkill.Acrobatics);
				m_SkillProficiencies.Add(DnDSkill.Stealth);
			}
			else if (characterClass.Name == "Cleric" || characterClass.Name == "Paladin")
			{
				m_SkillProficiencies.Add(DnDSkill.Religion);
				m_SkillProficiencies.Add(DnDSkill.Medicine);
			}
			else
			{
				m_SkillProficiencies.Add(DnDSkill.Athletics);
				m_SkillProficiencies.Add(DnDSkill.Survival);
			}

			Hits = HitsMax;

			RestoreAllSpellSlots();
		}

		#region Spell slots

		private int[] m_SpellSlotsUsed = new int[Spellcasting.MaxSpellLevel];

		public int GetMaxSpellSlots(int spellLevel)
		{
			if (!m_DnDInitialized || m_Classes.Count == 0)
			{
				return 0;
			}

			int totalCasterLevel = 0;
			int pactMagicLevel = 0;

			foreach (var kv in m_Classes)
			{
				if (kv.Key.SpellProgression == SpellProgression.Full) totalCasterLevel += kv.Value;
				else if (kv.Key.SpellProgression == SpellProgression.Half) totalCasterLevel += kv.Value / 2;
				else if (kv.Key.SpellProgression == SpellProgression.Third) totalCasterLevel += kv.Value / 3;
				else if (kv.Key.SpellProgression == SpellProgression.Pact) pactMagicLevel += kv.Value;
			}

			int multiclassSlots = Spellcasting.GetMaxSlots(SpellProgression.Full, totalCasterLevel, spellLevel);
			int pactSlots = pactMagicLevel > 0 ? Spellcasting.GetMaxSlots(SpellProgression.Pact, pactMagicLevel, spellLevel) : 0;

			if (m_Classes.Count == 1 && pactMagicLevel == 0)
			{
				foreach(var kv in m_Classes)
					return Spellcasting.GetMaxSlots(kv.Key.SpellProgression, kv.Value, spellLevel);
			}

			return multiclassSlots + pactSlots;
		}

		public int GetAvailableSpellSlots(int spellLevel)
		{
			if (spellLevel < 1 || spellLevel > Spellcasting.MaxSpellLevel)
			{
				return 0;
			}

			return Math.Max(0, GetMaxSpellSlots(spellLevel) - m_SpellSlotsUsed[spellLevel - 1]);
		}

		public bool ConsumeSpellSlot(int spellLevel)
		{
			if (GetAvailableSpellSlots(spellLevel) <= 0)
			{
				return false;
			}

			++m_SpellSlotsUsed[spellLevel - 1];
			return true;
		}

		public int FindSlotFor(int spellLevel)
		{
			for (int level = Math.Max(1, spellLevel); level <= Spellcasting.MaxSpellLevel; ++level)
			{
				if (GetAvailableSpellSlots(level) > 0)
				{
					return level;
				}
			}

			return 0;
		}

		public void RestoreAllSpellSlots()
		{
			for (int i = 0; i < m_SpellSlotsUsed.Length; ++i)
			{
				m_SpellSlotsUsed[i] = 0;
			}
		}

		public void LongRest()
		{
			Hits = HitsMax;
			RestoreAllSpellSlots();
			SendMessage(0x35, "You finish a long rest.");
			if (NetState != null)
			{
				NetState.Send(new DnDStatSync(this));
			}
		}

		public void ShortRest()
		{
			// Pact magic recovers on short rest. For multiclassing we'd have to track slots separately.
			// As a simplification for now, if they have pact magic we just restore all.
			bool hasPact = false;
			foreach(var kv in m_Classes)
			{
				if (kv.Key.SpellProgression == SpellProgression.Pact) hasPact = true;
			}

			if (hasPact)
			{
				RestoreAllSpellSlots();
			}

			SendMessage(0x35, "You finish a short rest.");
			if (NetState != null)
			{
				NetState.Send(new DnDStatSync(this));
			}
		}

		#endregion

		public override bool OnEquip(Item item)
		{
			if (m_DnDInitialized && AccessLevel < AccessLevel.GameMaster)
			{
				bool proficient = false;
				foreach (var kv in m_Classes)
				{
					if (kv.Key.IsProficientWith(item))
					{
						proficient = true;
						break;
					}
				}

				if (!proficient)
				{
					SendMessage("You are not proficient with that equipment.");
					return false;
				}

				Items.DnDArmor armor = item as Items.DnDArmor;

				if (armor != null && armor.MinimumStrength > m_AbilityScores.Str)
				{
					SendMessage(
						"You need a Strength of {0} to wear that; yours is {1}.",
						armor.MinimumStrength,
						m_AbilityScores.Str);

					return false;
				}
			}

			return base.OnEquip(item);
		}

		protected override void OnRaceChange(Race oldRace)
		{
			base.OnRaceChange(oldRace);

			Race race = Race;

			if (race == null)
			{
				return;
			}

			if (HairItemID != 0 && !race.ValidateHair(this, HairItemID))
			{
				HairItemID = 0;
			}

			if (FacialHairItemID != 0 && !race.ValidateFacialHair(this, FacialHairItemID))
			{
				FacialHairItemID = 0;
			}
		}

		public override void ComputeBaseLightLevels(out int global, out int personal)
		{
			global = LightCycleGlobal;
			bool darkvision = m_DnDInitialized && (Race as IDnDSpecies)?.HasDarkvision == true;
			personal = darkvision ? 30 : LightCyclePersonal;
		}

		private const int LightCycleGlobal = 0;
		private const int LightCyclePersonal = 21;

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)6); // version 6

			writer.Write(m_DnDInitialized);

			if (m_DnDInitialized)
			{
				m_AbilityScores.Serialize(writer);
				
				writer.Write(m_Classes.Count);
				foreach (var kv in m_Classes)
				{
					writer.Write(kv.Key.Name);
					writer.Write(kv.Value);
				}

				writer.Write(m_PrimaryClass == null ? "" : m_PrimaryClass.Name);

				writer.Write(m_PendingLevels);

				writer.Write(m_Feats.Count);
				foreach (var f in m_Feats)
				{
					writer.Write(f.Name);
				}

				writer.Write(m_SpellSlotsUsed.Length);
				for (int i = 0; i < m_SpellSlotsUsed.Length; ++i)
				{
					writer.Write(m_SpellSlotsUsed[i]);
				}

				writer.Write(m_Experience);

				writer.Write(m_SkillProficiencies.Count);
				foreach (DnDSkill skill in m_SkillProficiencies)
				{
					writer.Write((int)skill);
				}

				writer.Write(m_PendingAbilityScorePoints);
				writer.Write(m_PendingSpellsKnown);
				writer.Write(m_KnownSpells.Count);
				foreach (int spellId in m_KnownSpells)
				{
					writer.Write(spellId);
				}

				writer.WriteItemList(m_AttunedItems);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_DnDInitialized = reader.ReadBool();

			if (m_DnDInitialized)
			{
				m_AbilityScores = AbilityScores.Deserialize(reader);

				if (version >= 6)
				{
					int count = reader.ReadInt();
					for (int i = 0; i < count; i++)
					{
						CharacterClass c = CharacterClass.Parse(reader.ReadString());
						int lvl = reader.ReadInt();
						if (c != null) m_Classes[c] = lvl;
					}

					m_PrimaryClass = CharacterClass.Parse(reader.ReadString());
					m_PendingLevels = reader.ReadInt();

					int featCount = reader.ReadInt();
					for (int i = 0; i < featCount; i++)
					{
						Feat f = Feat.Parse(reader.ReadString());
						if (f != null) m_Feats.Add(f);
					}
				}
				else
				{
					CharacterClass singleClass = CharacterClass.Parse(reader.ReadString());
					int singleLevel = reader.ReadInt();
					if (singleClass != null)
					{
						m_Classes[singleClass] = singleLevel;
						m_PrimaryClass = singleClass;
					}
				}

				if (version >= 1)
				{
					int count = reader.ReadInt();

					for (int i = 0; i < count; ++i)
					{
						int used = reader.ReadInt();
						if (i < m_SpellSlotsUsed.Length)
						{
							m_SpellSlotsUsed[i] = used;
						}
					}
				}

				if (version >= 2)
				{
					m_Experience = reader.ReadInt();
				}

				if (version >= 3)
				{
					int count = reader.ReadInt();
					for (int i = 0; i < count; i++)
					{
						m_SkillProficiencies.Add((DnDSkill)reader.ReadInt());
					}
				}

				if (version >= 4)
				{
					m_PendingAbilityScorePoints = reader.ReadInt();
					m_PendingSpellsKnown = reader.ReadInt();
					int count = reader.ReadInt();
					for (int i = 0; i < count; i++)
					{
						m_KnownSpells.Add(reader.ReadInt());
					}
				}

				if (version >= 5)
				{
					m_AttunedItems = reader.ReadStrongItemList();
				}
			}
		}
	}
}
