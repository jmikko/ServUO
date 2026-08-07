#region References
using System;
#endregion

namespace Server
{
	public enum AbilityScoreType
	{
		Str,
		Dex,
		Con,
		Int,
		Wis,
		Cha
	}

	public struct AbilityScores
	{
		public int Str { get; set; }
		public int Dex { get; set; }
		public int Con { get; set; }
		public int Int { get; set; }
		public int Wis { get; set; }
		public int Cha { get; set; }

		public AbilityScores(int str, int dex, int con, int intl, int wis, int cha)
		{
			Str = str;
			Dex = dex;
			Con = con;
			Int = intl;
			Wis = wis;
			Cha = cha;
		}

		public static int Modifier(int score)
		{
			return (int)Math.Floor((score - 10) / 2.0);
		}

		public int StrMod { get { return Modifier(Str); } }
		public int DexMod { get { return Modifier(Dex); } }
		public int ConMod { get { return Modifier(Con); } }
		public int IntMod { get { return Modifier(Int); } }
		public int WisMod { get { return Modifier(Wis); } }
		public int ChaMod { get { return Modifier(Cha); } }

		public void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); // version

			writer.Write(Str);
			writer.Write(Dex);
			writer.Write(Con);
			writer.Write(Int);
			writer.Write(Wis);
			writer.Write(Cha);
		}

		public static AbilityScores Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();

			AbilityScores scores = new AbilityScores(
				reader.ReadInt(),
				reader.ReadInt(),
				reader.ReadInt(),
				reader.ReadInt(),
				reader.ReadInt(),
				reader.ReadInt());

			return scores;
		}
	}
}
