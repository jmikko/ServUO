using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blestsessebe corpse")]
    public sealed class SrdBlestsessebe : SrdMonster
    {
        [Constructable]
        public SrdBlestsessebe() : base("Blestsessebe") 
        {
        }

        public SrdBlestsessebe(Serial serial) : base(serial) { }
    }
}
