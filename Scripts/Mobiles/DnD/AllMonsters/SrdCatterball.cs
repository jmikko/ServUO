using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a catterball corpse")]
    public sealed class SrdCatterball : SrdMonster
    {
        [Constructable]
        public SrdCatterball() : base("Catterball") 
        {
        }

        public SrdCatterball(Serial serial) : base(serial) { }
    }
}
