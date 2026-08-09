using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a catamount corpse")]
    public sealed class SrdCatamount : SrdMonster
    {
        [Constructable]
        public SrdCatamount() : base("Catamount") 
        {
        }

        public SrdCatamount(Serial serial) : base(serial) { }
    }
}
