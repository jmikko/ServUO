using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a badger, giant corpse")]
    public sealed class SrdBadgerGiant : SrdMonster
    {
        [Constructable]
        public SrdBadgerGiant() : base("BadgerGiant") 
        {
        }

        public SrdBadgerGiant(Serial serial) : base(serial) { }
    }
}
