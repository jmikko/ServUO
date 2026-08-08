using System;

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

		/// <summary>Level 1 SRD hit points: hit die + Con modifier.</summary>
		public override int HitsMax
		{
			get
			{
				if (m_DnDInitialized && m_CharacterClass != null)
				{
					return Math.Max(1, m_CharacterClass.HitDie + m_AbilityScores.ConMod);
				}

				return 10;
			}
		}

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
		}

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

			writer.Write(0); // version

			writer.Write(m_DnDInitialized);

			if (m_DnDInitialized)
			{
				m_AbilityScores.Serialize(writer);
				writer.Write(m_CharacterClass == null ? "" : m_CharacterClass.Name);
				writer.Write(m_CharacterLevel);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt(); // version

			m_DnDInitialized = reader.ReadBool();

			if (m_DnDInitialized)
			{
				m_AbilityScores = AbilityScores.Deserialize(reader);
				m_CharacterClass = CharacterClass.Parse(reader.ReadString());
				m_CharacterLevel = reader.ReadInt();
			}
		}
	}
}
