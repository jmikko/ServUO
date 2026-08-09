using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crinaea corpse")]
    public sealed class SrdCrinaea : SrdMonster
    {
        [Constructable]
        public SrdCrinaea() : base("Crinaea") 
        {
        }

        public SrdCrinaea(Serial serial) : base(serial) { }
    }
}
