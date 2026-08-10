using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Mobiles
{
	/// <summary>
	/// Data record for one SRD monster, loaded from Data/DnDMonsters.xml.
	/// </summary>
	public sealed class SrdMonsterData
	{
		public string Id;
		public string DisplayName;
		public string CorpseName;
		public int Body;
		public int Hue;
		public int SoundID;
		public DnDAggression Aggression;

		public int ArmorClass;
		public int AttackBonus;
		public string DamageDice;
		public int HitPoints;

		/// <summary>
		/// SRD challenge rating, which is the only difficulty measure a D&amp;D monster has - it is
		/// what the experience award is derived from. This replaces UO's Fame/Karma, which drove
		/// title, murder and reputation systems that no longer exist here.
		/// </summary>
		public double ChallengeRating;

		/// <summary>
		/// What this creature does that others do not. Never null - a row with no trait attributes
		/// gets a default block, so nothing has to null-check before asking about resistances.
		/// </summary>
		public DnDMonsterTraits Traits = new DnDMonsterTraits();

		public List<DnDAction> Actions = new List<DnDAction>();
		public List<DnDLegendaryAction> LegendaryActions = new List<DnDLegendaryAction>();
		// Reactions could be just a list of DnDAction or a string/custom object. For now we use strings
		public List<string> Reactions = new List<string>();
	}

	/// <summary>
	/// Shared base for every SRD monster. Per the data-driven approach recommended for bulk D&amp;D
	/// content (rather than one hand-written stat-block class per monster), all the actual numbers
	/// live in Data/DnDMonsters.xml; concrete subclasses (SrdOrc, SrdZombie, etc.) are just a few
	/// lines each providing the [Constructable] entry point ServUO's spawner system needs (it
	/// resolves spawns by C# type name via reflection, so a fully generic single class can't be
	/// spawned directly - see Scripts/Regions/Spawning/SpawnDefinition.cs).
	///
	/// Legacy UO stats (Str/Dex/Int/Skills) are deliberately not set here: D&amp;D combat resolution
	/// (BaseWeapon.CheckHit/ComputeDamage) is unconditional now and never reads them for monsters,
	/// so carrying them would just be unused "just UO" baggage. Mobile.Hits/HitsMax are set directly
	/// from the same HP value IDnDCreature.HitPointsMaxDnD reports, rather than maintaining two
	/// parallel HP tracks.
	/// </summary>
	public abstract class SrdMonster : DnDCreature, IDnDTraited
	{
		private static readonly Dictionary<string, List<SrdMonsterData>> m_Data = new Dictionary<string, List<SrdMonsterData>>();

		public static void Configure()
		{
			string path = Path.Combine(Core.BaseDirectory, "Data", "DnDMonsters.xml");

			if (!File.Exists(path))
			{
				Console.WriteLine("Warning: {0} does not exist, no SRD monsters loaded", path);
				return;
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(path);

			XmlNodeList nodes = doc.SelectNodes("//monster");

			foreach (XmlNode node in nodes)
			{
				XmlElement el = node as XmlElement;

				if (el == null)
				{
					continue;
				}

				SrdMonsterData data = new SrdMonsterData
				{
					Id = el.GetAttribute("id"),
					DisplayName = el.GetAttribute("name"),
					CorpseName = el.GetAttribute("corpse"),
					Body = ParseInt(el.GetAttribute("body")),
					Hue = ParseInt(el.GetAttribute("hue")),
					SoundID = ParseInt(el.GetAttribute("sound")),
					Aggression = el.GetAttribute("fightMode") == "Aggressor" ? DnDAggression.Defensive : DnDAggression.Hostile,
					ArmorClass = ParseInt(el.GetAttribute("ac")),
					AttackBonus = ParseInt(el.GetAttribute("attackBonus")),
					DamageDice = el.GetAttribute("damageDice"),
					HitPoints = ParseInt(el.GetAttribute("hp")),
					ChallengeRating = Advancement.ParseChallengeRating(el.GetAttribute("cr")),
					Traits = ParseTraits(el)
				};

				XmlElement scoresEl = el["abilityScores"];
				if (scoresEl != null)
				{
					data.Traits.Scores = new AbilityScores(
						ParseInt(scoresEl.GetAttribute("str")),
						ParseInt(scoresEl.GetAttribute("dex")),
						ParseInt(scoresEl.GetAttribute("con")),
						ParseInt(scoresEl.GetAttribute("int")),
						ParseInt(scoresEl.GetAttribute("wis")),
						ParseInt(scoresEl.GetAttribute("cha"))
					);
				}

				XmlElement actionsEl = el["actions"];
				if (actionsEl != null)
				{
					foreach (XmlNode actionNode in actionsEl.ChildNodes)
					{
						XmlElement actionEl = actionNode as XmlElement;
						if (actionEl == null || actionEl.Name != "action") continue;

						DnDAction action = new DnDAction();
						action.Name = actionEl.GetAttribute("name");
						Enum.TryParse(actionEl.GetAttribute("type"), true, out DnDActionType actType);
						action.Type = actType;
						action.ToHit = ParseInt(actionEl.GetAttribute("toHit"));
						action.ReachOrRange = actionEl.GetAttribute("reachOrRange");
						action.PrimaryDamageDice = actionEl.GetAttribute("primaryDamageDice");
						action.PrimaryDamageType = DnDMonsterTraits.ParseDamageTypes(actionEl.GetAttribute("primaryDamageType"), data.Id, "primaryDamageType");
						Enum.TryParse(actionEl.GetAttribute("usage"), true, out DnDUsageType usage);
						action.Usage = usage;

						data.Actions.Add(action);
					}
				}

				// Backwards compatibility for AttackBonus / DamageDice if not provided but actions are present
				// IsNullOrEmpty, not == null: XmlElement.GetAttribute returns "" for an attribute
				// that is not there, never null, so the null test never fired and every migrated
				// monster fell through with no damage dice at all.
				if (data.AttackBonus == 0 && string.IsNullOrEmpty(data.DamageDice) && data.Actions.Count > 0)
				{
					data.AttackBonus = data.Actions[0].ToHit;
					data.DamageDice = data.Actions[0].PrimaryDamageDice;
				}

				List<SrdMonsterData> list;
				if (!m_Data.TryGetValue(data.Id, out list))
				{
					list = new List<SrdMonsterData>();
					m_Data[data.Id] = list;
				}

				list.Add(data);
			}

			int totalRows = 0;
			foreach (var list in m_Data.Values)
			{
				totalRows += list.Count;
			}

			Console.WriteLine("SrdMonster: loaded {0} monster stat blocks across {1} ids from Data/DnDMonsters.xml", totalRows, m_Data.Count);
		}

		/// <summary>
		/// Reads the trait attributes off a row. Every one is optional: a row with none of them
		/// gets a default trait block, which is a plain Medium creature with no resistances - what
		/// every monster in the game was before these existed.
		/// </summary>
		private static DnDMonsterTraits ParseTraits(XmlElement el)
		{
			string id = el.GetAttribute("id");

			var traits = new DnDMonsterTraits();

			string size = el.GetAttribute("size");
			string type = el.GetAttribute("type");

			if (!string.IsNullOrEmpty(size))
			{
				traits.Size = size;
			}

			if (!string.IsNullOrEmpty(type))
			{
				traits.Type = type;
			}

			int speed = ParseInt(el.GetAttribute("speed"));

			if (speed > 0)
			{
				traits.Speed = speed;
			}

			traits.FlySpeed = ParseInt(el.GetAttribute("flySpeed"));
			traits.SwimSpeed = ParseInt(el.GetAttribute("swimSpeed"));
			traits.BurrowSpeed = ParseInt(el.GetAttribute("burrowSpeed"));
			traits.ClimbSpeed = ParseInt(el.GetAttribute("climbSpeed"));

			traits.Resistances = DnDMonsterTraits.ParseDamageTypes(el.GetAttribute("resist"), id, "resist");
			traits.Immunities = DnDMonsterTraits.ParseDamageTypes(el.GetAttribute("immune"), id, "immune");
			traits.Vulnerabilities = DnDMonsterTraits.ParseDamageTypes(el.GetAttribute("vulnerable"), id, "vulnerable");

			traits.ConditionImmunities = DnDMonsterTraits.ParseConditions(el.GetAttribute("conditionImmune"), id);

			traits.Darkvision = ParseInt(el.GetAttribute("darkvision"));
			traits.Blindsight = ParseInt(el.GetAttribute("blindsight"));
			traits.Truesight = ParseInt(el.GetAttribute("truesight"));
			traits.Tremorsense = ParseInt(el.GetAttribute("tremorsense"));

			traits.PackTactics = el.GetAttribute("packTactics") == "true";
			traits.MagicResistance = el.GetAttribute("magicResistance") == "true";
			traits.Regeneration = ParseInt(el.GetAttribute("regeneration"));

			int multiattack = ParseInt(el.GetAttribute("multiattack"));

			if (multiattack > 1)
			{
				traits.Multiattack = multiattack;
			}

			traits.KeenSenses = el.GetAttribute("keenSenses") == "true";
			traits.SunlightSensitivity = el.GetAttribute("sunlightSensitivity") == "true";
			traits.UndeadFortitude = el.GetAttribute("undeadFortitude") == "true";
			traits.Amphibious = el.GetAttribute("amphibious") == "true";
			traits.Incorporeal = el.GetAttribute("incorporeal") == "true";

			traits.Flavour = el.GetAttribute("flavour");

			// The natural damage type now lives on the creature's first action, since an action
			// list is a better place for it than a second copy on the element. The old flat
			// attribute is still read as a fallback so a row written either way works.
			traits.NaturalDamageType =
				DnDMonsterTraits.ParseDamageTypes(el.GetAttribute("damageType"), id, "damageType");

			if (traits.NaturalDamageType == DnDDamageType.None)
			{
				XmlNodeList actionNodes = el.SelectNodes("actions/action");

				if (actionNodes != null && actionNodes.Count > 0)
				{
					var first = actionNodes[0] as XmlElement;

					if (first != null)
					{
						traits.NaturalDamageType = DnDMonsterTraits.ParseDamageTypes(
							first.GetAttribute("primaryDamageType"), id, "primaryDamageType");
					}
				}
			}

			return traits;
		}

		private static int ParseInt(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}

			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				return Convert.ToInt32(value.Substring(2), 16);
			}

			int result;
			int.TryParse(value, out result);
			return result;
		}

		/// <summary>The stat block for an id, for anything that needs one - Wild Shape, mainly. Returns the first variant.</summary>
		public static SrdMonsterData Lookup(string id)
		{
			List<SrdMonsterData> list;

			return m_Data.TryGetValue(id, out list) && list.Count > 0 ? list[0] : null;
		}

		public static System.Collections.Generic.IEnumerable<SrdMonsterData> AllData 
		{ 
			get 
			{ 
				foreach (var list in m_Data.Values)
				{
					foreach (var data in list)
					{
						yield return data;
					}
				}
			} 
		}

		private static List<SrdMonsterData> GetDataList(string id)
		{
			List<SrdMonsterData> list;

			if (!m_Data.TryGetValue(id, out list) || list.Count == 0)
			{
				throw new InvalidOperationException(
					"SrdMonster: no data for id '" + id + "' - is Data/DnDMonsters.xml missing an entry, " +
					"or did SrdMonster.Configure() not run before this monster was constructed?");
			}

			return list;
		}

		private string m_MonsterId;
		private int m_VariantIndex;

		public List<DnDAction> Actions { get { return GetDataList(m_MonsterId)[m_VariantIndex].Actions; } }

		protected SrdMonster(string monsterId)
			: base(GetDataList(monsterId)[Utility.Random(GetDataList(monsterId).Count)].Aggression)
		{
			m_MonsterId = monsterId;

			List<SrdMonsterData> list = GetDataList(monsterId);
			m_VariantIndex = Utility.Random(list.Count);

			SrdMonsterData data = list[m_VariantIndex];

			Name = data.DisplayName;
			Body = data.Body;

			if (data.Hue != 0)
			{
				Hue = data.Hue;
			}

			BaseSoundID = data.SoundID;

			Hits = data.HitPoints;

			VirtualArmor = data.ArmorClass;
		}

		protected SrdMonster(Serial serial)
			: base(serial)
		{
		}

		public DnDMonsterTraits Traits { get { return GetDataList(m_MonsterId)[m_VariantIndex].Traits; } }

		/// <summary>
		/// Adds the creature's one sentence of character under its name.
		/// <para>
		/// A player meeting a creature for the first time has no stat block to read, so this is
		/// the only place the game gets to say what it is - and a line about how a thing fights is
		/// worth more at that moment than its armour class.
		/// </para>
		/// </summary>
		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			DnDMonsterTraits traits = Traits;

			if (traits != null && !string.IsNullOrEmpty(traits.Flavour))
			{
				list.Add(1042971, traits.Flavour);
			}
		}

		public override int ArmorClass { get { return GetDataList(m_MonsterId)[m_VariantIndex].ArmorClass; } }
		public override int AttackBonus { get { return GetDataList(m_MonsterId)[m_VariantIndex].AttackBonus; } }
		public override string DamageDiceExpression { get { return GetDataList(m_MonsterId)[m_VariantIndex].DamageDice; } }
		public override int HitPointsMaxDnD { get { return GetDataList(m_MonsterId)[m_VariantIndex].HitPoints; } }
		public override double ChallengeRating { get { return GetDataList(m_MonsterId)[m_VariantIndex].ChallengeRating; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version
			writer.Write(m_MonsterId);
			writer.Write(m_VariantIndex);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_MonsterId = reader.ReadString();

			if (version >= 1)
			{
				m_VariantIndex = reader.ReadInt();
			}
			else
			{
				m_VariantIndex = 0;
			}
			
			// Bound check just in case the XML was changed to have fewer variants
			List<SrdMonsterData> list;
			if (m_Data.TryGetValue(m_MonsterId, out list) && m_VariantIndex >= list.Count)
			{
				m_VariantIndex = 0;
			}
		}
	}
}
