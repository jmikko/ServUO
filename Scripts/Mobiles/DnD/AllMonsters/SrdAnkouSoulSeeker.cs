using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ankou soul seeker corpse")]
    public sealed class SrdAnkouSoulSeeker : SrdMonster
    {
        [Constructable]
        public SrdAnkouSoulSeeker() : base("AnkouSoulSeeker") 
        {
        }

        public SrdAnkouSoulSeeker(Serial serial) : base(serial) { }
    }
}
