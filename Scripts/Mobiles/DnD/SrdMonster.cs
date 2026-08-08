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
		public int Fame;
		public int Karma;
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
	public abstract class SrdMonster : DnDCreature
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
					Fame = ParseInt(el.GetAttribute("fame")),
					Karma = ParseInt(el.GetAttribute("karma"))
				};

				m_Data[data.Id] = data;
			}

			Console.WriteLine("SrdMonster: loaded {0} monster stat blocks from Data/DnDMonsters.xml", m_Data.Count);
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

			Fame = data.Fame;
			Karma = data.Karma;

			VirtualArmor = data.ArmorClass;
		}

		protected SrdMonster(Serial serial)
			: base(serial)
		{
		}

		public override int ArmorClass { get { return GetData(m_MonsterId).ArmorClass; } }
		public override int AttackBonus { get { return GetData(m_MonsterId).AttackBonus; } }
		public override string DamageDiceExpression { get { return GetData(m_MonsterId).DamageDice; } }
		public override int HitPointsMaxDnD { get { return GetData(m_MonsterId).HitPoints; } }

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
