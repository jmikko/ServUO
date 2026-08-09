using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deva corpse")]
    public sealed class SrdDeva : SrdMonster
    {
        [Constructable]
        public SrdDeva() : base("Deva") 
        {
        }

        public SrdDeva(Serial serial) : base(serial) { }
    }
}
