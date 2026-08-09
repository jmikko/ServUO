using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bloodsapper corpse")]
    public sealed class SrdBloodsapper : SrdMonster
    {
        [Constructable]
        public SrdBloodsapper() : base("Bloodsapper") 
        {
        }

        public SrdBloodsapper(Serial serial) : base(serial) { }
    }
}
