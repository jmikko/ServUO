using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bone crab corpse")]
    public sealed class SrdBoneCrab : SrdMonster
    {
        [Constructable]
        public SrdBoneCrab() : base("BoneCrab") 
        {
        }

        public SrdBoneCrab(Serial serial) : base(serial) { }
    }
}
