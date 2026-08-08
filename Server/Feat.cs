using System;
using System.Collections.Generic;

namespace Server
{
    public abstract class Feat
    {
        private static readonly List<Feat> m_AllFeats = new List<Feat>();
        public static List<Feat> AllFeats { get { return m_AllFeats; } }

        public static void Register(Feat feat)
        {
            if (!m_AllFeats.Contains(feat))
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
        
        /// <summary>
        /// Can the given character select this feat?
        /// </summary>
        public virtual bool CanSelect(IDnDCharacter character)
        {
            return true;
        }

        /// <summary>
        /// Any one-time setup (e.g. adding proficiencies or spells).
        /// </summary>
        public virtual void OnSelected(IDnDCharacter character)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
