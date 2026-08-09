using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave giant shaman corpse")]
    public sealed class SrdCaveGiantShaman : SrdMonster
    {
        [Constructable]
        public SrdCaveGiantShaman() : base("CaveGiantShaman") 
        {
        }

        public SrdCaveGiantShaman(Serial serial) : base(serial) { }
    }
}
