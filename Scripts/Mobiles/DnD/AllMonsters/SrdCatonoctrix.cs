using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a catonoctrix corpse")]
    public sealed class SrdCatonoctrix : SrdMonster
    {
        [Constructable]
        public SrdCatonoctrix() : base("Catonoctrix") 
        {
        }

        public SrdCatonoctrix(Serial serial) : base(serial) { }
    }
}
