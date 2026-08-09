using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil, moldering corpse")]
    public sealed class SrdDevilMoldering : SrdMonster
    {
        [Constructable]
        public SrdDevilMoldering() : base("DevilMoldering") 
        {
        }

        public SrdDevilMoldering(Serial serial) : base(serial) { }
    }
}
