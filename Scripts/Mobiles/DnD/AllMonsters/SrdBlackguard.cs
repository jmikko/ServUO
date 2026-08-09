using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blackguard corpse")]
    public sealed class SrdBlackguard : SrdMonster
    {
        [Constructable]
        public SrdBlackguard() : base("Blackguard") 
        {
        }

        public SrdBlackguard(Serial serial) : base(serial) { }
    }
}
