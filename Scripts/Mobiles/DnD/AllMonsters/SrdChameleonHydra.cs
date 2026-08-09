using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chameleon hydra corpse")]
    public sealed class SrdChameleonHydra : SrdMonster
    {
        [Constructable]
        public SrdChameleonHydra() : base("ChameleonHydra") 
        {
        }

        public SrdChameleonHydra(Serial serial) : base(serial) { }
    }
}
