using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cosmopolitan alchemist corpse")]
    public sealed class SrdCosmopolitanAlchemist : SrdMonster
    {
        [Constructable]
        public SrdCosmopolitanAlchemist() : base("CosmopolitanAlchemist") 
        {
        }

        public SrdCosmopolitanAlchemist(Serial serial) : base(serial) { }
    }
}
