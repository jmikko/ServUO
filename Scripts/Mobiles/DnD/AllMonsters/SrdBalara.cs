using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a balara corpse")]
    public sealed class SrdBalara : SrdMonster
    {
        [Constructable]
        public SrdBalara() : base("Balara") 
        {
        }

        public SrdBalara(Serial serial) : base(serial) { }
    }
}
