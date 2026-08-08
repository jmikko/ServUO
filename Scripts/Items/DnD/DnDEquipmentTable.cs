using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Server.Items
{
	public sealed class DnDWeaponData
	{
		public string Id;
		public string Name;
		public WeaponCategory Category;
		public string Damage;
		public string VersatileDamage;
		public DamageType DamageType;
		public WeaponProperty Properties;
		public int ItemID;
		public Layer Layer;
		public double Weight;
		public int Cost;
		public int Range;

		public bool Has(WeaponProperty property)
		{
			return (Properties & property) != 0;
		}
	}

	public sealed class DnDArmorData
	{
		public string Id;
		public string Name;
		public ArmorCategory Category;
		public int BaseArmorClass;

		/// <summary>-1 means the Dexterity modifier applies in full.</summary>
		public int MaxDexBonus;

		public int MinimumStrength;
		public bool StealthDisadvantage;
		public int ItemID;
		public Layer Layer;
		public double Weight;
		public int Cost;
	}

	/// <summary>
	/// Loads the SRD weapon and armour tables from Data/DnDWeapons.xml and Data/DnDArmor.xml.
	/// <para>
	/// These are rows in a table, not behaviours, so they live as data rather than as one class per
	/// item - the same reasoning as Data/DnDMonsters.xml. ServUO's spawner and [add resolve by C#
	/// type name through reflection, so each entry still needs a thin concrete class; those are
	/// generated in DnDWeapons.cs and DnDArmors.cs.
	/// </para>
	/// </summary>
	public static class DnDEquipmentTable
	{
		private static readonly Dictionary<string, DnDWeaponData> m_Weapons =
			new Dictionary<string, DnDWeaponData>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<string, DnDArmorData> m_Armor =
			new Dictionary<string, DnDArmorData>(StringComparer.OrdinalIgnoreCase);

		public static IEnumerable<DnDWeaponData> Weapons { get { return m_Weapons.Values; } }
		public static IEnumerable<DnDArmorData> Armor { get { return m_Armor.Values; } }

		public static void Configure()
		{
			LoadWeapons(Path.Combine(Core.BaseDirectory, "Data", "DnDWeapons.xml"));
			LoadArmor(Path.Combine(Core.BaseDirectory, "Data", "DnDArmor.xml"));

			Console.WriteLine(
				"Equipment: loaded {0} weapon(s) and {1} armour piece(s).", m_Weapons.Count, m_Armor.Count);
		}

		public static DnDWeaponData GetWeapon(string id)
		{
			DnDWeaponData data;

			if (m_Weapons.TryGetValue(id, out data))
			{
				return data;
			}

			throw new InvalidOperationException(
				String.Format("No weapon '{0}' in Data/DnDWeapons.xml", id));
		}

		public static DnDArmorData GetArmor(string id)
		{
			DnDArmorData data;

			if (m_Armor.TryGetValue(id, out data))
			{
				return data;
			}

			throw new InvalidOperationException(
				String.Format("No armour '{0}' in Data/DnDArmor.xml", id));
		}

		private static void LoadWeapons(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("Warning: {0} does not exist, no weapons loaded", path);
				return;
			}

			var doc = new XmlDocument();
			doc.Load(path);

			foreach (XmlElement el in doc.SelectNodes("//weapon"))
			{
				var data = new DnDWeaponData
				{
					Id = el.GetAttribute("id"),
					Name = el.GetAttribute("name"),
					Category = ParseEnum(el.GetAttribute("category"), WeaponCategory.SimpleMelee),
					Damage = el.GetAttribute("damage"),
					VersatileDamage = el.GetAttribute("versatile"),
					DamageType = ParseEnum(el.GetAttribute("type"), DamageType.Bludgeoning),
					Properties = ParseProperties(el.GetAttribute("properties")),
					ItemID = ParseInt(el.GetAttribute("itemID")),
					Layer = ParseEnum(el.GetAttribute("layer"), Layer.OneHanded),
					Weight = ParseDouble(el.GetAttribute("weight")),
					Cost = ParseInt(el.GetAttribute("cost")),
					Range = ParseInt(el.GetAttribute("range"))
				};

				m_Weapons[data.Id] = data;
			}
		}

		private static void LoadArmor(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("Warning: {0} does not exist, no armour loaded", path);
				return;
			}

			var doc = new XmlDocument();
			doc.Load(path);

			foreach (XmlElement el in doc.SelectNodes("//piece"))
			{
				var data = new DnDArmorData
				{
					Id = el.GetAttribute("id"),
					Name = el.GetAttribute("name"),
					Category = ParseEnum(el.GetAttribute("category"), ArmorCategory.Light),
					BaseArmorClass = ParseInt(el.GetAttribute("baseAC")),
					MaxDexBonus = ParseInt(el.GetAttribute("maxDex"), -1),
					MinimumStrength = ParseInt(el.GetAttribute("minStr")),
					StealthDisadvantage = el.GetAttribute("stealth") == "true",
					ItemID = ParseInt(el.GetAttribute("itemID")),
					Layer = ParseEnum(el.GetAttribute("layer"), Layer.InnerTorso),
					Weight = ParseDouble(el.GetAttribute("weight")),
					Cost = ParseInt(el.GetAttribute("cost"))
				};

				m_Armor[data.Id] = data;
			}
		}

		private static WeaponProperty ParseProperties(string value)
		{
			WeaponProperty result = WeaponProperty.None;

			if (String.IsNullOrEmpty(value))
			{
				return result;
			}

			foreach (string part in value.Split(','))
			{
				result |= ParseEnum(part.Trim(), WeaponProperty.None);
			}

			return result;
		}

		private static T ParseEnum<T>(string value, T fallback) where T : struct
		{
			T result;

			return !String.IsNullOrEmpty(value) && Enum.TryParse(value, true, out result) ? result : fallback;
		}

		/// <summary>Accepts both plain and 0x-prefixed values, since item IDs are written in hex.</summary>
		private static int ParseInt(string value, int fallback = 0)
		{
			if (String.IsNullOrEmpty(value))
			{
				return fallback;
			}

			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				int hex;

				return Int32.TryParse(value.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hex)
					? hex
					: fallback;
			}

			int result;

			return Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result) ? result : fallback;
		}

		private static double ParseDouble(string value)
		{
			double result;

			return !String.IsNullOrEmpty(value) &&
				   double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result)
				? result
				: 1.0;
		}
	}
}
