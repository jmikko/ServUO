using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black shuck corpse")]
    public sealed class SrdBlackShuck : SrdMonster
    {
        [Constructable]
        public SrdBlackShuck() : base("BlackShuck") 
        {
        }

        public SrdBlackShuck(Serial serial) : base(serial) { }
    }
}
