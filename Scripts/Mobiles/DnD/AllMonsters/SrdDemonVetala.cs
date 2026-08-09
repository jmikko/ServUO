using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a demon, vetala corpse")]
    public sealed class SrdDemonVetala : SrdMonster
    {
        [Constructable]
        public SrdDemonVetala() : base("DemonVetala") 
        {
        }

        public SrdDemonVetala(Serial serial) : base(serial) { }
    }
}
