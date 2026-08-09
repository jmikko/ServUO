using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a caldera kite corpse")]
    public sealed class SrdCalderaKite : SrdMonster
    {
        [Constructable]
        public SrdCalderaKite() : base("CalderaKite") 
        {
        }

        public SrdCalderaKite(Serial serial) : base(serial) { }
    }
}
