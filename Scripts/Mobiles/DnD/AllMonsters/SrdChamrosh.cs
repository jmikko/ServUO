using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chamrosh corpse")]
    public sealed class SrdChamrosh : SrdMonster
    {
        [Constructable]
        public SrdChamrosh() : base("Chamrosh") 
        {
        }

        public SrdChamrosh(Serial serial) : base(serial) { }
    }
}
