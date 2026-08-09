using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil, maelstrom corpse")]
    public sealed class SrdDevilMaelstrom : SrdMonster
    {
        [Constructable]
        public SrdDevilMaelstrom() : base("DevilMaelstrom") 
        {
        }

        public SrdDevilMaelstrom(Serial serial) : base(serial) { }
    }
}
