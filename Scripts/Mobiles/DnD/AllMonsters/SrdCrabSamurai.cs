using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crab, samurai corpse")]
    public sealed class SrdCrabSamurai : SrdMonster
    {
        [Constructable]
        public SrdCrabSamurai() : base("CrabSamurai") 
        {
        }

        public SrdCrabSamurai(Serial serial) : base(serial) { }
    }
}
