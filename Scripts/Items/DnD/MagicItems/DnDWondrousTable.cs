using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Items
{
	public class DnDWondrousData
	{
		public string Id;
		public string Name;
		public bool RequiresAttunement;

		public int ArmorClassBonus;
		public int AttackBonus;
		public int DamageBonus;
		public int SavingThrowBonus;

		/// <summary>
		/// Scores this item sets, indexed by AbilityScoreType. Zero means the item says nothing
		/// about that ability - which is different from setting it to zero, and is why this is a
		/// sparse override rather than a bonus.
		/// </summary>
		public int[] AbilityOverrides = new int[6];

		public int ItemID;
		public Layer Layer;
		public int Hue;
		public double Weight;
		public int Cost;
	}

	/// <summary>
	/// Loads the wondrous item table from Data/DnDMagicItems.xml.
	/// <para>
	/// Same reasoning as the weapon and armour tables: an item whose entire effect is "+1 armour
	/// class and +1 to saves while attuned" is a row, not a class. The two accessories that existed
	/// before this were fifty lines of C# apiece for exactly that, and a catalogue built that way
	/// stops growing at about four items - which is where it had stopped.
	/// </para>
	/// </summary>
	public static class DnDWondrousTable
	{
		private static readonly Dictionary<string, DnDWondrousData> m_Items =
			new Dictionary<string, DnDWondrousData>(StringComparer.OrdinalIgnoreCase);

		public static IEnumerable<DnDWondrousData> Items { get { return m_Items.Values; } }

		public static int Count { get { return m_Items.Count; } }

		public static void Configure()
		{
			Load(Path.Combine(Core.BaseDirectory, "Data", "DnDMagicItems.xml"));

			Console.WriteLine("Equipment: loaded {0} wondrous item(s).", m_Items.Count);
		}

		public static DnDWondrousData Get(string id)
		{
			DnDWondrousData data;

			if (m_Items.TryGetValue(id, out data))
			{
				return data;
			}

			throw new InvalidOperationException(
				String.Format("No magic item '{0}' in Data/DnDMagicItems.xml", id));
		}

		private static void Load(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("Warning: {0} does not exist, no magic items loaded", path);
				return;
			}

			var doc = new XmlDocument();
			doc.Load(path);

			foreach (XmlElement el in doc.SelectNodes("//item"))
			{
				var data = new DnDWondrousData
				{
					Id = el.GetAttribute("id"),
					Name = el.GetAttribute("name"),
					RequiresAttunement = el.GetAttribute("attunement") == "true",
					ArmorClassBonus = ParseInt(el.GetAttribute("ac")),
					AttackBonus = ParseInt(el.GetAttribute("attack")),
					DamageBonus = ParseInt(el.GetAttribute("damage")),
					SavingThrowBonus = ParseInt(el.GetAttribute("save")),
					ItemID = ParseInt(el.GetAttribute("itemID")),
					Layer = ParseLayer(el.GetAttribute("layer")),
					Hue = ParseInt(el.GetAttribute("hue")),
					Weight = ParseDouble(el.GetAttribute("weight")),
					Cost = ParseInt(el.GetAttribute("cost"))
				};

				data.AbilityOverrides[(int)AbilityScoreType.Str] = ParseInt(el.GetAttribute("str"));
				data.AbilityOverrides[(int)AbilityScoreType.Dex] = ParseInt(el.GetAttribute("dex"));
				data.AbilityOverrides[(int)AbilityScoreType.Con] = ParseInt(el.GetAttribute("con"));
				data.AbilityOverrides[(int)AbilityScoreType.Int] = ParseInt(el.GetAttribute("int"));
				data.AbilityOverrides[(int)AbilityScoreType.Wis] = ParseInt(el.GetAttribute("wis"));
				data.AbilityOverrides[(int)AbilityScoreType.Cha] = ParseInt(el.GetAttribute("cha"));

				m_Items[data.Id] = data;
			}
		}

		private static int ParseInt(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				return 0;
			}

			// Item ids in the table are written as hex, the way every other equipment table does.
			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				return Convert.ToInt32(value.Substring(2), 16);
			}

			int result;

			if (Int32.TryParse(value, out result))
			{
				return result;
			}

			// Returning 0 quietly is how a Flame Tongue written as damage="2d6" ended up with no
			// damage bonus at all: the row looked right, the item did nothing, and nothing said so.
			// These columns are flat numbers - dice belong to the weapon table.
			Console.WriteLine(
				"Warning: '{0}' in Data/DnDMagicItems.xml is not a number - these columns are flat bonuses", value);

			return 0;
		}

		private static double ParseDouble(string value)
		{
			double result;

			return Double.TryParse(
				value, System.Globalization.NumberStyles.Any,
				System.Globalization.CultureInfo.InvariantCulture, out result) ? result : 1.0;
		}

		private static Layer ParseLayer(string value)
		{
			try
			{
				return (Layer)Enum.Parse(typeof(Layer), value, true);
			}
			catch
			{
				return Layer.Ring;
			}
		}
	}
}
