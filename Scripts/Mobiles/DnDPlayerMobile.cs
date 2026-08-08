using System;
using Server.Network;

namespace Server.Mobiles
{
	/// <summary>
	/// The player character.
	/// <para>
	/// Like <see cref="DnDCreature"/>, this sits directly on <see cref="Mobile"/> rather than
	/// porting ServUO's PlayerMobile. That class is ~12k lines and threads bulk-order timers,
	/// champion titles, virtue contexts, community-collection points, quest state, faction and
	/// VvV membership through its serializer - all legacy UO systems with no D&amp;D equivalent,
	/// and all of them dragging their subsystems back in behind them.
	/// </para>
	/// What remains here is the D&amp;D character sheet: ability scores, class, level, and the
	/// derived AC / HP the rules core in Server/ reads through <see cref="IDnDCharacter"/>.
	/// </summary>
	public class DnDPlayerMobile : Mobile, IDnDCharacter
	{
		private bool m_DnDInitialized;
		private AbilityScores m_AbilityScores;
		private CharacterClass m_CharacterClass;
		private int m_CharacterLevel;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool DnDInitialized { get { return m_DnDInitialized; } }

		public AbilityScores AbilityScores { get { return m_AbilityScores; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public CharacterClass CharacterClass { get { return m_CharacterClass; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int CharacterLevel { get { return m_CharacterLevel; } }

		public DnDPlayerMobile()
		{
		}

		public DnDPlayerMobile(Serial serial)
			: base(serial)
		{
		}

		public override bool NewGuildDisplay { get { return true; } }

		#region Account bookkeeping
		/// <summary>When the current play session began; Account totals game time from this.</summary>
		public DateTime SessionStart { get; set; }

		/// <summary>Accumulated played time across sessions.</summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan GameTime { get; set; }

		/// <summary>
		/// UO's "young player" new-player protection has no D&amp;D equivalent and is not
		/// implemented. Account still reads and clears the flag, so it is kept as an
		/// always-false property whose setter is a no-op.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Young { get { return false; } set { } }
		#endregion

		/// <summary>
		/// SRD armour class. Unarmoured is 10 + Dex mod; body armour replaces the 10 with its own
		/// base and caps the Dex contribution (medium +2, heavy +0); a shield adds on top.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster)]
		public int ArmorClass
		{
			get
			{
				int baseAC = 10;
				int shieldBonus = 0;
				int maxDex = int.MaxValue;

				foreach (Item item in Items)
				{
					IDnDEquipment eq = item as IDnDEquipment;

					if (eq == null || eq.ArmorCategory == ArmorCategory.None)
					{
						continue;
					}

					if (eq.ArmorCategory == ArmorCategory.Shield)
					{
						shieldBonus += eq.ArmorBonus;
					}
					else
					{
						baseAC = eq.ArmorBonus;

						if (eq.ArmorCategory == ArmorCategory.Medium)
						{
							maxDex = 2;
						}
						else if (eq.ArmorCategory == ArmorCategory.Heavy)
						{
							maxDex = 0;
						}
					}
				}

				int dexMod = Math.Min(m_AbilityScores.DexMod, maxDex);

				return baseAC + dexMod + shieldBonus;
			}
		}

		/// <summary>
		/// SRD hit points: the full hit die at 1st level, the die's average each level after, plus
		/// the Constitution modifier every level.
		/// </summary>
		public override int HitsMax
		{
			get
			{
				if (m_DnDInitialized && m_CharacterClass != null)
				{
					return Advancement.GetMaxHitPoints(
						m_CharacterClass.HitDie, m_AbilityScores.ConMod, m_CharacterLevel);
				}

				return 10;
			}
		}

		#region Advancement

		private int m_Experience;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Experience { get { return m_Experience; } }

		/// <summary>
		/// Adds experience and applies every level it earns. Levelling is not a choice a player
		/// makes here - crossing the threshold applies it immediately, which keeps hit points,
		/// proficiency bonus and spell slots from ever disagreeing with the experience total.
		/// </summary>
		public void AwardExperience(int amount)
		{
			if (!m_DnDInitialized || amount <= 0 || m_CharacterLevel >= Advancement.MaxLevel)
			{
				return;
			}

			m_Experience += amount;

			SendMessage("You gain {0} experience.", amount);

			int newLevel = Advancement.GetLevelForExperience(m_Experience);

			while (m_CharacterLevel < newLevel)
			{
				++m_CharacterLevel;
				OnLevelUp();
			}
		}

		/// <summary>
		/// A new level is worth its hit points immediately - the character gains the difference in
		/// current hit points as well as maximum, so levelling up is never a reason to go and rest.
		/// New spell slots come from the level itself; unspent ones are not refilled.
		/// </summary>
		private void OnLevelUp()
		{
			int before = Hits;

			Hits = Math.Min(HitsMax, before + GainedHitPointsThisLevel());

			SendMessage(0x35, "You are now level {0}.", m_CharacterLevel);

			Delta(MobileDelta.Hits);

			if (NetState != null)
			{
				NetState.Send(new DnDStatSync(this));
			}

			// A level can unlock a whole new spell level, so the list is resent, not just the slots.
			Spells.DnD.DnDSpellPackets.SendSpellList(this);
		}

		private int GainedHitPointsThisLevel()
		{
			if (m_CharacterClass == null)
			{
				return 0;
			}

			return Advancement.GetMaxHitPoints(m_CharacterClass.HitDie, m_AbilityScores.ConMod, m_CharacterLevel) -
				   Advancement.GetMaxHitPoints(m_CharacterClass.HitDie, m_AbilityScores.ConMod, m_CharacterLevel - 1);
		}

		#endregion

		public void ApplyDnDSetup(AbilityScores scores, CharacterClass characterClass)
		{
			if (m_DnDInitialized)
			{
				return; // one-time only; guards against a replayed setup packet
			}

			m_AbilityScores = scores;
			m_CharacterClass = characterClass;
			m_CharacterLevel = 1;
			m_DnDInitialized = true;

			Hits = HitsMax;

			RestoreAllSpellSlots();
		}

		#region Spell slots

		// Index 0 holds 1st-level slots; cantrips cost nothing and so are not tracked here.
		private int[] m_SpellSlotsUsed = new int[Spellcasting.MaxSpellLevel];

		public int GetMaxSpellSlots(int spellLevel)
		{
			if (!m_DnDInitialized || m_CharacterClass == null)
			{
				return 0;
			}

			return Spellcasting.GetMaxSlots(m_CharacterClass.SpellProgression, m_CharacterLevel, spellLevel);
		}

		public int GetAvailableSpellSlots(int spellLevel)
		{
			if (spellLevel < 1 || spellLevel > Spellcasting.MaxSpellLevel)
			{
				return 0;
			}

			return Math.Max(0, GetMaxSpellSlots(spellLevel) - m_SpellSlotsUsed[spellLevel - 1]);
		}

		/// <summary>Spends one slot of the given level, or reports that there wasn't one.</summary>
		public bool ConsumeSpellSlot(int spellLevel)
		{
			if (GetAvailableSpellSlots(spellLevel) <= 0)
			{
				return false;
			}

			++m_SpellSlotsUsed[spellLevel - 1];
			return true;
		}

		/// <summary>
		/// The lowest unspent slot that can carry a spell of this level, or 0 if there is none.
		/// Casting from the smallest slot that fits is what a player would pick by hand.
		/// </summary>
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

		/// <summary>
		/// A long rest: hit points to full and every spell slot back.
		/// </summary>
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

		/// <summary>
		/// A short rest. Only Pact Magic comes back - that is the whole point of the Warlock's small
		/// slot pool. Hit dice spending is not modelled yet, so this restores no hit points.
		/// </summary>
		public void ShortRest()
		{
			if (m_CharacterClass != null && m_CharacterClass.SpellProgression == SpellProgression.Pact)
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

		/// <summary>
		/// D&amp;D proficiency is enforced at the actual equip boundary, rather than only when the
		/// initial kit is granted. This also covers gear received from future loot, vendors, or
		/// GM-created test items.
		/// </summary>
		public override bool OnEquip(Item item)
		{
			if (m_DnDInitialized && AccessLevel < AccessLevel.GameMaster &&
				m_CharacterClass != null && !m_CharacterClass.IsProficientWith(item))
			{
				SendMessage("You are not proficient with that equipment.");
				return false;
			}

			return base.OnEquip(item);
		}

		/// <summary>Species darkvision, surfaced through the light-level calculation.</summary>
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

			writer.Write(2); // version

			writer.Write(m_DnDInitialized);

			if (m_DnDInitialized)
			{
				m_AbilityScores.Serialize(writer);
				writer.Write(m_CharacterClass == null ? "" : m_CharacterClass.Name);
				writer.Write(m_CharacterLevel);

				// version 1: spent spell slots
				writer.Write(m_SpellSlotsUsed.Length);

				for (int i = 0; i < m_SpellSlotsUsed.Length; ++i)
				{
					writer.Write(m_SpellSlotsUsed[i]);
				}

				writer.Write(m_Experience); // version 2
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
				m_CharacterClass = CharacterClass.Parse(reader.ReadString());
				m_CharacterLevel = reader.ReadInt();

				if (version >= 1)
				{
					int count = reader.ReadInt();

					for (int i = 0; i < count; ++i)
					{
						int used = reader.ReadInt();

						// Tolerate a save written when MaxSpellLevel was larger than it is now.
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
			}
		}
	}
}
