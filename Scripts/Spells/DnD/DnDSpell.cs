using System;
using System.Collections.Generic;
using Server.Engines.Classes;
using Server.Mobiles;

namespace Server.Spells.DnD
{
	/// <summary>
	/// How a spell decides whether it affects its target.
	/// </summary>
	public enum SpellResolution
	{
		/// <summary>No roll - the spell simply works (Magic Missile, most healing).</summary>
		Automatic,

		/// <summary>A spell attack roll against the target's AC.</summary>
		SpellAttack,

		/// <summary>The target rolls a saving throw against the caster's spell save DC.</summary>
		SavingThrow
	}

	public enum SpellTargetType
	{
		Mobile,
		Location,
		Item
	}

	/// <summary>
	/// One SRD spell.
	/// <para>
	/// This replaces UO's Spell/SpellCircle machinery outright. There is no mana pool, no casting
	/// skill check, no reagents and no fizzle: a caster either has an unspent slot of the right
	/// level or the spell does not go off. Cantrips cost nothing and scale on character level.
	/// </para>
	/// </summary>
	public abstract class DnDSpell
	{
		public abstract string Name { get; }

		/// <summary>0 for a cantrip.</summary>
		public abstract int Level { get; }

		public abstract SpellSchool School { get; }

		public virtual SpellResolution Resolution { get { return SpellResolution.Automatic; } }

		/// <summary>Only consulted when <see cref="Resolution"/> is SavingThrow.</summary>
		public virtual AbilityScoreType SaveAbility { get { return AbilityScoreType.Dex; } }

		/// <summary>Range in tiles. 0 means self or touch.</summary>
		public virtual int Range { get { return 12; } }

		/// <summary>A successful save halves the damage rather than negating it.</summary>
		public virtual bool HalfDamageOnSave { get { return false; } }

		/// <summary>The shape of the spell's area, if it has one.</summary>
		public virtual SpellShape Shape { get { return SpellShape.Single; } }

		/// <summary>
		/// The size of that shape in tiles - a sphere's radius, a cone or line's length, a cube's
		/// half-extent. One tile is 5 feet, so an SRD 15-foot cone is 3.
		/// </summary>
		public virtual int AreaSize { get { return 0; } }

		/// <summary>Whether holding this spell occupies the caster's concentration.</summary>
		public virtual bool RequiresConcentration { get { return false; } }

		/// <summary>How long a concentration or timed spell lasts.</summary>
		public virtual TimeSpan Duration { get { return TimeSpan.Zero; } }

		public bool IsCantrip { get { return Level == 0; } }

		/// <summary>
		/// Applies the spell. <paramref name="slotLevel"/> is the level of the slot actually spent,
		/// which is what upcasting reads - it equals <see cref="Level"/> unless the caster chose to
		/// burn something bigger.
		/// </summary>
		public abstract void Effect(Mobile caster, IDnDCharacter character, Mobile targetMobile, Point3D targetLocation, int slotLevel);

		/// <summary>Whether this spell needs a target other than the caster.</summary>
		public virtual bool RequiresTarget { get { return true; } }

		public virtual SpellTargetType TargetType { get { return SpellTargetType.Mobile; } }

		/// <summary>
		/// Whether the spell helps rather than harms. Used to keep healing off enemies and damage
		/// off yourself without every spell writing its own check.
		/// </summary>
		public virtual bool Beneficial { get { return false; } }

		public override string ToString()
		{
			return Name;
		}
	}

	/// <summary>
	/// Every spell in the game, by name, plus which classes may learn each one.
	/// </summary>
	public static class SpellRegistry
	{
		private static readonly Dictionary<string, DnDSpell> m_Spells =
			new Dictionary<string, DnDSpell>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<string, List<DnDSpell>> m_ClassLists =
			new Dictionary<string, List<DnDSpell>>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Registration order, which is what gives every spell the small integer id the wire
		/// protocol uses. Ids are therefore only stable for as long as SrdSpells.Configure keeps
		/// registering in the same order - fine while client and server ship together, and the
		/// thing to replace with an explicit id if they ever stop.
		/// </summary>
		private static readonly List<DnDSpell> m_Ordered = new List<DnDSpell>();

		public static IEnumerable<DnDSpell> AllSpells { get { return m_Spells.Values; } }

		public static int Count { get { return m_Spells.Count; } }

		public static void Register(DnDSpell spell, params string[] classNames)
		{
			if (spell == null)
			{
				return;
			}

			m_Spells[spell.Name] = spell;

			if (!m_Ordered.Contains(spell))
			{
				m_Ordered.Add(spell);
			}

			foreach (string className in classNames)
			{
				List<DnDSpell> list;

				if (!m_ClassLists.TryGetValue(className, out list))
				{
					m_ClassLists[className] = list = new List<DnDSpell>();
				}

				if (!list.Contains(spell))
				{
					list.Add(spell);
				}
			}
		}

		public static DnDSpell Find(string name)
		{
			DnDSpell spell;

			return name != null && m_Spells.TryGetValue(name, out spell) ? spell : null;
		}

		public static DnDSpell FindById(int id)
		{
			return id >= 0 && id < m_Ordered.Count ? m_Ordered[id] : null;
		}

		/// <summary>-1 if the spell was never registered.</summary>
		public static int GetId(DnDSpell spell)
		{
			return m_Ordered.IndexOf(spell);
		}

		/// <summary>Every spell on a class' list, whether or not the character is high enough level.</summary>
		public static List<DnDSpell> GetClassList(string className)
		{
			List<DnDSpell> list;

			if (className != null && m_ClassLists.TryGetValue(className, out list))
			{
				return list;
			}

			return new List<DnDSpell>();
		}

		/// <summary>The spells a specific character can actually cast right now.</summary>
		public static List<DnDSpell> GetAvailable(IDnDCharacter character)
		{
			var available = new List<DnDSpell>();

			if (character == null || character.PrimaryClass == null)
			{
				return available;
			}

			int highest = Spellcasting.GetHighestSlotLevel(
				character.PrimaryClass.SpellProgression, character.TotalLevel);

			int maxSpells = character.PrimaryClass.GetSpellsKnown(character.TotalLevel);
			bool knowsAll = (maxSpells == int.MaxValue);

			DnDPlayerMobile pm = character as DnDPlayerMobile;
			List<int> knownIds = pm != null ? pm.KnownSpells : new List<int>();

			foreach (DnDSpell spell in GetClassList(character.PrimaryClass.Name))
			{
				if (spell.IsCantrip || spell.Level <= highest)
				{
					if (spell.IsCantrip || knowsAll || knownIds.Contains(GetId(spell)))
					{
						available.Add(spell);
					}
				}
			}

			return available;
		}

		/// <summary>Spells the character could learn (on class list, castable level, not a cantrip, not already known).</summary>
		public static List<DnDSpell> GetLearnable(IDnDCharacter character)
		{
			var learnable = new List<DnDSpell>();

			if (character == null || character.PrimaryClass == null)
			{
				return learnable;
			}

			int highest = Spellcasting.GetHighestSlotLevel(
				character.PrimaryClass.SpellProgression, character.TotalLevel);

			DnDPlayerMobile pm = character as DnDPlayerMobile;
			List<int> knownIds = pm != null ? pm.KnownSpells : new List<int>();

			foreach (DnDSpell spell in GetClassList(character.PrimaryClass.Name))
			{
				if (!spell.IsCantrip && spell.Level <= highest && !knownIds.Contains(GetId(spell)))
				{
					learnable.Add(spell);
				}
			}

			return learnable;
		}
	}
}
