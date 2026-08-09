using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a efreeti noble corpse")]
    public sealed class SrdEfreetiNoble : SrdMonster
    {
        [Constructable]
        public SrdEfreetiNoble() : base("EfreetiNoble") 
        {
        }

        public SrdEfreetiNoble(Serial serial) : base(serial) { }
    }
}
