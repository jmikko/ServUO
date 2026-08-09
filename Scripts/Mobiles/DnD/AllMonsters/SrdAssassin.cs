using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a assassin corpse")]
    public sealed class SrdAssassin : SrdMonster
    {
        [Constructable]
        public SrdAssassin() : base("Assassin") 
        {
        }

        public SrdAssassin(Serial serial) : base(serial) { }
    }
}
