using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a astri corpse")]
    public sealed class SrdAstri : SrdMonster
    {
        [Constructable]
        public SrdAstri() : base("Astri") 
        {
        }

        public SrdAstri(Serial serial) : base(serial) { }
    }
}
