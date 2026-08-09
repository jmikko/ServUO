using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a balor general corpse")]
    public sealed class SrdBalorGeneral : SrdMonster
    {
        [Constructable]
        public SrdBalorGeneral() : base("BalorGeneral") 
        {
        }

        public SrdBalorGeneral(Serial serial) : base(serial) { }
    }
}
