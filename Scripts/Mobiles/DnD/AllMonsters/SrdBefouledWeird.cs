using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a befouled weird corpse")]
    public sealed class SrdBefouledWeird : SrdMonster
    {
        [Constructable]
        public SrdBefouledWeird() : base("BefouledWeird") 
        {
        }

        public SrdBefouledWeird(Serial serial) : base(serial) { }
    }
}
