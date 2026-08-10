using System;
using System.Collections.Generic;

namespace Server
{
	/// <summary>The eleven SRD damage types, as a flag set so a monster's resistances are one field.</summary>
	[Flags]
	public enum DnDDamageType
	{
		None = 0x0000,
		Bludgeoning = 0x0001,
		Piercing = 0x0002,
		Slashing = 0x0004,
		Fire = 0x0008,
		Cold = 0x0010,
		Lightning = 0x0020,
		Thunder = 0x0040,
		Acid = 0x0080,
		Poison = 0x0100,
		Necrotic = 0x0200,
		Radiant = 0x0400,
		Psychic = 0x0800,
		Force = 0x1000,

		Physical = Bludgeoning | Piercing | Slashing
	}

	/// <summary>
	/// What makes one monster fight differently from another.
	/// <para>
	/// Before this, every creature in the game was six numbers - armour class, hit points, attack
	/// bonus, damage dice, body, sound - and so every fight was arithmetic with a different picture
	/// on top. A skeleton and a wolf of the same challenge rating were the same encounter. These
	/// are the fields that make a skeleton brittle to a mace and immune to poison, a wolf dangerous
	/// in a pack, and a troll something you have to finish rather than merely beat.
	/// </para>
	/// <para>
	/// Held as data on the stat block rather than as behaviour on a class, for the same reason the
	/// stat blocks themselves are data: there are hundreds of these, and a class each would be
	/// hundreds of files nobody can hold in their head. Every field here is consulted by a rule
	/// that already exists - the attack roll, the damage application, the condition table - so
	/// adding one to a row changes the fight without changing any code.
	/// </para>
	/// </summary>
	public sealed class DnDMonsterTraits
	{
		public string Size = "Medium";
		public string Type = "Monstrosity";

		public int Speed = 30;
		public int FlySpeed;
		public int SwimSpeed;
		public int BurrowSpeed;
		public int ClimbSpeed;

		public DnDDamageType Resistances;
		public DnDDamageType Immunities;
		public DnDDamageType Vulnerabilities;

		public DnDCondition ConditionImmunities;

		public AbilityScores Scores;
		public Dictionary<AbilityScoreType, int> SavingThrows = new Dictionary<AbilityScoreType, int>();
		public Dictionary<string, int> Skills = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		public int PassivePerception;
		public string Languages = string.Empty;

		public int Darkvision;
		public int Blindsight;
		public int Truesight;
		public int Tremorsense;

		/// <summary>Advantage on attacks when an ally is also next to the target - wolves, goblins.</summary>
		public bool PackTactics;

		/// <summary>Advantage on saving throws against spells.</summary>
		public bool MagicResistance;

		/// <summary>Hit points regained per round. Trolls, and the reason fire matters against them.</summary>
		public int Regeneration;

		/// <summary>Attacks per action. One unless the stat block says otherwise.</summary>
		public int Multiattack = 1;

		public bool KeenSenses;
		public bool SunlightSensitivity;
		public bool UndeadFortitude;
		public bool Amphibious;
		public bool Incorporeal;

		/// <summary>One sentence of character, shown when the creature is examined.</summary>
		public string Flavour = string.Empty;

		/// <summary>
		/// What this creature's own attacks are made of - claws, bite, slam, or something worse.
		/// <para>
		/// A monster has no weapon item to carry a type, so without this every natural attack is
		/// untyped and resistances only ever bite against armed players. That is a strange
		/// half-rule: a skeleton would resist a player's mace and not an ogre's club.
		/// </para>
		/// </summary>
		public DnDDamageType NaturalDamageType = DnDDamageType.None;

		/// <summary>
		/// How a damage type lands: 0 for immune, half for resistant, double for vulnerable.
		/// <para>
		/// Rounded down, and floored at zero rather than one - immunity has to mean immunity, or a
		/// fire elemental takes chip damage from a torch and the word stops meaning anything.
		/// </para>
		/// </summary>
		public int ApplyDamageType(int damage, DnDDamageType type)
		{
			if (type == DnDDamageType.None || damage <= 0)
			{
				return damage;
			}

			if ((Immunities & type) != 0)
			{
				return 0;
			}

			if ((Vulnerabilities & type) != 0)
			{
				damage *= 2;
			}

			if ((Resistances & type) != 0)
			{
				damage /= 2;
			}

			return damage;
		}

		private static readonly Dictionary<string, DnDDamageType> m_DamageNames =
			new Dictionary<string, DnDDamageType>(StringComparer.OrdinalIgnoreCase)
			{
				{ "bludgeoning", DnDDamageType.Bludgeoning },
				{ "piercing", DnDDamageType.Piercing },
				{ "slashing", DnDDamageType.Slashing },
				{ "fire", DnDDamageType.Fire },
				{ "cold", DnDDamageType.Cold },
				{ "lightning", DnDDamageType.Lightning },
				{ "thunder", DnDDamageType.Thunder },
				{ "acid", DnDDamageType.Acid },
				{ "poison", DnDDamageType.Poison },
				{ "necrotic", DnDDamageType.Necrotic },
				{ "radiant", DnDDamageType.Radiant },
				{ "psychic", DnDDamageType.Psychic },
				{ "force", DnDDamageType.Force }
			};

		private static readonly Dictionary<string, DnDCondition> m_ConditionNames =
			new Dictionary<string, DnDCondition>(StringComparer.OrdinalIgnoreCase)
			{
				{ "blinded", DnDCondition.Blinded },
				{ "charmed", DnDCondition.Charmed },
				{ "deafened", DnDCondition.Deafened },
				{ "frightened", DnDCondition.Frightened },
				{ "grappled", DnDCondition.Grappled },
				{ "incapacitated", DnDCondition.Incapacitated },
				{ "paralyzed", DnDCondition.Paralyzed },
				{ "petrified", DnDCondition.Petrified },
				{ "poisoned", DnDCondition.Poisoned },
				{ "prone", DnDCondition.Prone },
				{ "restrained", DnDCondition.Restrained },
				{ "stunned", DnDCondition.Stunned },
				{ "unconscious", DnDCondition.Unconscious }
			};

		/// <summary>
		/// Parses a comma-separated damage-type list, reporting anything it does not recognise.
		/// <para>
		/// Loudly, because these lists are written by hand across hundreds of rows and a typo
		/// would otherwise mean a monster quietly loses an immunity - which looks exactly like the
		/// monster being balanced that way on purpose.
		/// </para>
		/// </summary>
		public static DnDDamageType ParseDamageTypes(string value, string monsterId, string field)
		{
			var result = DnDDamageType.None;

			if (string.IsNullOrEmpty(value))
			{
				return result;
			}

			foreach (string part in value.Split(','))
			{
				string name = part.Trim();

				if (name.Length == 0)
				{
					continue;
				}

				DnDDamageType type;

				if (m_DamageNames.TryGetValue(name, out type))
				{
					result |= type;
				}
				else
				{
					Console.WriteLine(
						"Warning: monster '{0}' has an unknown damage type '{1}' in {2}", monsterId, name, field);
				}
			}

			return result;
		}

		public static DnDCondition ParseConditions(string value, string monsterId)
		{
			var result = DnDCondition.None;

			if (string.IsNullOrEmpty(value))
			{
				return result;
			}

			foreach (string part in value.Split(','))
			{
				string name = part.Trim();

				if (name.Length == 0)
				{
					continue;
				}

				DnDCondition condition;

				if (m_ConditionNames.TryGetValue(name, out condition))
				{
					result |= condition;
				}
				else
				{
					Console.WriteLine(
						"Warning: monster '{0}' has an unknown condition '{1}' in conditionImmune", monsterId, name);
				}
			}

			return result;
		}
	}
}
