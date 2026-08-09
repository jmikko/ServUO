using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crab corpse")]
    public sealed class SrdCrab : SrdMonster
    {
        [Constructable]
        public SrdCrab() : base("Crab") 
        {
        }

        public SrdCrab(Serial serial) : base(serial) { }
    }
}
