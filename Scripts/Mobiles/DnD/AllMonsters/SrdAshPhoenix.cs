using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ash phoenix corpse")]
    public sealed class SrdAshPhoenix : SrdMonster
    {
        [Constructable]
        public SrdAshPhoenix() : base("AshPhoenix") 
        {
        }

        public SrdAshPhoenix(Serial serial) : base(serial) { }
    }
}
