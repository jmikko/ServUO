using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave troll corpse")]
    public sealed class SrdCaveTroll : SrdMonster
    {
        [Constructable]
        public SrdCaveTroll() : base("CaveTroll") 
        {
        }

        public SrdCaveTroll(Serial serial) : base(serial) { }
    }
}
