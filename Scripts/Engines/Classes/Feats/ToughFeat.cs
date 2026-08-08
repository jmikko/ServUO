using System;

namespace Server.Engines.Classes.Feats
{
    public class ToughFeat : Feat
    {
        public override string Name { get { return "Tough"; } }

        public override bool CanSelect(IDnDCharacter character)
        {
            // The Tough feat can be taken multiple times? No, feats can only be taken once unless specified.
            if (character.Feats != null)
            {
                foreach (var f in character.Feats)
                {
                    if (f is ToughFeat) return false;
                }
            }
            return true;
        }
    }
}
