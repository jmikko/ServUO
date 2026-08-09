using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a astral devourer corpse")]
    public sealed class SrdAstralDevourer : SrdMonster
    {
        [Constructable]
        public SrdAstralDevourer() : base("AstralDevourer") 
        {
        }

        public SrdAstralDevourer(Serial serial) : base(serial) { }
    }
}
