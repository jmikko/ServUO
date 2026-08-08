using System;
using System.Collections.Generic;

namespace Server
{
    public abstract class Feat
    {
        private static readonly List<Feat> m_AllFeats = new List<Feat>();
        public static List<Feat> AllFeats { get { return m_AllFeats; } }

        /// <summary>
        /// Registers a feat, keyed by name rather than by reference.
        /// <para>
        /// Reference equality is the wrong test here: every caller passes a freshly constructed
        /// instance, so a registry that dedupes on Contains never dedupes at all. It only shows up
        /// if registration runs twice - which it did, the first time, because ScriptCompiler invokes
        /// every public static Configure it can find and the feat list was also being configured
        /// explicitly. Name is what Parse looks up by, so name is what uniqueness should mean.
        /// </para>
        /// </summary>
        public static void Register(Feat feat)
        {
            if (feat != null && Parse(feat.Name) == null)
            {
                m_AllFeats.Add(feat);
            }
        }

        public static Feat Parse(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            foreach (var f in m_AllFeats)
            {
                if (Insensitive.Equals(f.Name, name))
                    return f;
            }
            return null;
        }

        public abstract string Name { get; }

        public virtual string Description { get { return String.Empty; } }

        /// <summary>
        /// Can the given character select this feat?
        /// </summary>
        public virtual bool CanSelect(IDnDCharacter character)
        {
            return !HasFeat(character, GetType());
        }

        /// <summary>
        /// Any one-time setup (e.g. adding proficiencies or spells).
        /// </summary>
        public virtual void OnSelected(IDnDCharacter character)
        {
        }

        #region Rule hooks

        // These mirror ClassFeature's hooks, and for the same reason: without them every rule that
        // a feat can touch has to name the feat by type at its call site. HitsMax did exactly that
        // for Tough, and doing it once more for each of a dozen feats would spread the feat list
        // across combat, armour class and saving throws.

        /// <summary>Extra hit points per character level. Tough gives 2.</summary>
        public virtual int HitPointsPerLevel { get { return 0; } }

        public virtual int AttackBonus { get { return 0; } }

        public virtual int ArmorClassBonus { get { return 0; } }

        public virtual int DamageBonus { get { return 0; } }

        public virtual int InitiativeBonus { get { return 0; } }

        /// <summary>A flat bonus to every saving throw. Resilient's is narrower - see below.</summary>
        public virtual int SaveBonus { get { return 0; } }

        /// <summary>A bonus to saves of one ability only, which is what most save feats grant.</summary>
        public virtual int GetSaveBonus(AbilityScoreType ability) { return SaveBonus; }

        public virtual bool GrantsSaveAdvantage(AbilityScoreType ability) { return false; }

        /// <summary>Ability score increases, applied once when the feat is taken.</summary>
        public virtual int GetAbilityIncrease(AbilityScoreType ability) { return 0; }

        #endregion

        public static bool HasFeat(IDnDCharacter character, Type type)
        {
            if (character == null || character.Feats == null)
            {
                return false;
            }

            foreach (Feat f in character.Feats)
            {
                if (f != null && f.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Has(IDnDCharacter character, string name)
        {
            if (character == null || character.Feats == null)
            {
                return false;
            }

            foreach (Feat f in character.Feats)
            {
                if (f != null && Insensitive.Equals(f.Name, name))
                {
                    return true;
                }
            }

            return false;
        }

        #region Aggregation

        private static int Sum(IDnDCharacter character, Func<Feat, int> selector)
        {
            if (character == null || character.Feats == null)
            {
                return 0;
            }

            int total = 0;

            foreach (Feat f in character.Feats)
            {
                if (f != null)
                {
                    total += selector(f);
                }
            }

            return total;
        }

        public static int GetHitPointsPerLevel(IDnDCharacter character)
        {
            return Sum(character, f => f.HitPointsPerLevel);
        }

        public static int GetAttackBonus(IDnDCharacter character)
        {
            return Sum(character, f => f.AttackBonus);
        }

        public static int GetArmorClassBonus(IDnDCharacter character)
        {
            return Sum(character, f => f.ArmorClassBonus);
        }

        public static int GetDamageBonus(IDnDCharacter character)
        {
            return Sum(character, f => f.DamageBonus);
        }

        public static int GetInitiativeBonus(IDnDCharacter character)
        {
            return Sum(character, f => f.InitiativeBonus);
        }

        public static int GetSaveBonusFor(IDnDCharacter character, AbilityScoreType ability)
        {
            return Sum(character, f => f.GetSaveBonus(ability));
        }

        public static bool HasSaveAdvantage(IDnDCharacter character, AbilityScoreType ability)
        {
            if (character == null || character.Feats == null)
            {
                return false;
            }

            foreach (Feat f in character.Feats)
            {
                if (f != null && f.GrantsSaveAdvantage(ability))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
