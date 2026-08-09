using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a annelidast corpse")]
    public sealed class SrdAnnelidast : SrdMonster
    {
        [Constructable]
        public SrdAnnelidast() : base("Annelidast") 
        {
        }

        public SrdAnnelidast(Serial serial) : base(serial) { }
    }
}
