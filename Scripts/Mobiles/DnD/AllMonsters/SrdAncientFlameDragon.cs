using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient flame dragon corpse")]
    public sealed class SrdAncientFlameDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientFlameDragon() : base("AncientFlameDragon") 
        {
        }

        public SrdAncientFlameDragon(Serial serial) : base(serial) { }
    }
}
