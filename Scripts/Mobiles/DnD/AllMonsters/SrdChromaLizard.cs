using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chroma lizard corpse")]
    public sealed class SrdChromaLizard : SrdMonster
    {
        [Constructable]
        public SrdChromaLizard() : base("ChromaLizard") 
        {
        }

        public SrdChromaLizard(Serial serial) : base(serial) { }
    }
}
