using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brumalek corpse")]
    public sealed class SrdBrumalek : SrdMonster
    {
        [Constructable]
        public SrdBrumalek() : base("Brumalek") 
        {
        }

        public SrdBrumalek(Serial serial) : base(serial) { }
    }
}
