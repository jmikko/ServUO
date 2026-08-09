using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drider corpse")]
    public sealed class SrdDrider : SrdMonster
    {
        [Constructable]
        public SrdDrider() : base("Drider") 
        {
        }

        public SrdDrider(Serial serial) : base(serial) { }
    }
}
