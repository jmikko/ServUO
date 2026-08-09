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
		private static readonly Dictionary<string, SrdMonsterData> m_Data = new Dictionary<string, SrdMonsterData>();

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

				m_Data[data.Id] = data;
			}

			Console.WriteLine("SrdMonster: loaded {0} monster stat blocks from Data/DnDMonsters.xml", m_Data.Count);
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

		/// <summary>The stat block for an id, for anything that needs one - Wild Shape, mainly.</summary>
		public static SrdMonsterData Lookup(string id)
		{
			SrdMonsterData data;

			return m_Data.TryGetValue(id, out data) ? data : null;
		}

		public static System.Collections.Generic.IEnumerable<SrdMonsterData> AllData { get { return m_Data.Values; } }

		private static SrdMonsterData GetData(string id)
		{
			SrdMonsterData data;

			if (!m_Data.TryGetValue(id, out data))
			{
				throw new InvalidOperationException(
					"SrdMonster: no data for id '" + id + "' - is Data/DnDMonsters.xml missing an entry, " +
					"or did SrdMonster.Configure() not run before this monster was constructed?");
			}

			return data;
		}

		private string m_MonsterId;

		protected SrdMonster(string monsterId)
			: base(GetData(monsterId).Aggression)
		{
			m_MonsterId = monsterId;

			SrdMonsterData data = GetData(monsterId);

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

		public DnDMonsterTraits Traits { get { return GetData(m_MonsterId).Traits; } }

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

		public override int ArmorClass { get { return GetData(m_MonsterId).ArmorClass; } }
		public override int AttackBonus { get { return GetData(m_MonsterId).AttackBonus; } }
		public override string DamageDiceExpression { get { return GetData(m_MonsterId).DamageDice; } }
		public override int HitPointsMaxDnD { get { return GetData(m_MonsterId).HitPoints; } }
		public override double ChallengeRating { get { return GetData(m_MonsterId).ChallengeRating; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			writer.Write(m_MonsterId);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_MonsterId = reader.ReadString();
		}
	}
}
