using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a eala corpse")]
    public sealed class SrdEala : SrdMonster
    {
        [Constructable]
        public SrdEala() : base("Eala") 
        {
        }

        public SrdEala(Serial serial) : base(serial) { }
    }
}
