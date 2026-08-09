using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dread troll corpse")]
    public sealed class SrdDreadTroll : SrdMonster
    {
        [Constructable]
        public SrdDreadTroll() : base("DreadTroll") 
        {
        }

        public SrdDreadTroll(Serial serial) : base(serial) { }
    }
}
