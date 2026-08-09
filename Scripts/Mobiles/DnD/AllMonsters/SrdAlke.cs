using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alke corpse")]
    public sealed class SrdAlke : SrdMonster
    {
        [Constructable]
        public SrdAlke() : base("Alke") 
        {
        }

        public SrdAlke(Serial serial) : base(serial) { }
    }
}
