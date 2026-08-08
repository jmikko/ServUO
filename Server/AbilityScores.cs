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

		/// <summary>Reads one score by name, for rules that work on whichever ability they are told.</summary>
		public int Get(AbilityScoreType ability)
		{
			switch (ability)
			{
				case AbilityScoreType.Str: return Str;
				case AbilityScoreType.Dex: return Dex;
				case AbilityScoreType.Con: return Con;
				case AbilityScoreType.Int: return Int;
				case AbilityScoreType.Wis: return Wis;
				case AbilityScoreType.Cha: return Cha;
			}

			return 10;
		}

		public int GetModifier(AbilityScoreType ability)
		{
			return Modifier(Get(ability));
		}

		/// <summary>Adds to one score, capped at 20 - the limit the rules put on advancement.</summary>
		public AbilityScores Increase(AbilityScoreType ability, int amount)
		{
			int raised = Math.Min(20, Get(ability) + amount);

			switch (ability)
			{
				case AbilityScoreType.Str: return new AbilityScores(raised, Dex, Con, Int, Wis, Cha);
				case AbilityScoreType.Dex: return new AbilityScores(Str, raised, Con, Int, Wis, Cha);
				case AbilityScoreType.Con: return new AbilityScores(Str, Dex, raised, Int, Wis, Cha);
				case AbilityScoreType.Int: return new AbilityScores(Str, Dex, Con, raised, Wis, Cha);
				case AbilityScoreType.Wis: return new AbilityScores(Str, Dex, Con, Int, raised, Cha);
				case AbilityScoreType.Cha: return new AbilityScores(Str, Dex, Con, Int, Wis, raised);
			}

			return this;
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
