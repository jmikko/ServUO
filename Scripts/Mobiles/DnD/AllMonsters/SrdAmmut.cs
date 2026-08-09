using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ammut corpse")]
    public sealed class SrdAmmut : SrdMonster
    {
        [Constructable]
        public SrdAmmut() : base("Ammut") 
        {
        }

        public SrdAmmut(Serial serial) : base(serial) { }
    }
}
