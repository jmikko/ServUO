using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a astral snapper corpse")]
    public sealed class SrdAstralSnapper : SrdMonster
    {
        [Constructable]
        public SrdAstralSnapper() : base("AstralSnapper") 
        {
        }

        public SrdAstralSnapper(Serial serial) : base(serial) { }
    }
}
