using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a baleful miasma corpse")]
    public sealed class SrdBalefulMiasma : SrdMonster
    {
        [Constructable]
        public SrdBalefulMiasma() : base("BalefulMiasma") 
        {
        }

        public SrdBalefulMiasma(Serial serial) : base(serial) { }
    }
}
