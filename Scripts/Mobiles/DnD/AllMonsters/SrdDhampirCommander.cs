using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dhampir commander corpse")]
    public sealed class SrdDhampirCommander : SrdMonster
    {
        [Constructable]
        public SrdDhampirCommander() : base("DhampirCommander") 
        {
        }

        public SrdDhampirCommander(Serial serial) : base(serial) { }
    }
}
