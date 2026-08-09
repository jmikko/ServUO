using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a barbed devil corpse")]
    public sealed class SrdBarbedDevil : SrdMonster
    {
        [Constructable]
        public SrdBarbedDevil() : base("BarbedDevil") 
        {
        }

        public SrdBarbedDevil(Serial serial) : base(serial) { }
    }
}
