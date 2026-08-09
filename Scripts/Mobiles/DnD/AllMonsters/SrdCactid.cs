using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cactid corpse")]
    public sealed class SrdCactid : SrdMonster
    {
        [Constructable]
        public SrdCactid() : base("Cactid") 
        {
        }

        public SrdCactid(Serial serial) : base(serial) { }
    }
}
