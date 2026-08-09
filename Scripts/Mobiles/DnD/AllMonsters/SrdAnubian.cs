using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a anubian corpse")]
    public sealed class SrdAnubian : SrdMonster
    {
        [Constructable]
        public SrdAnubian() : base("Anubian") 
        {
        }

        public SrdAnubian(Serial serial) : base(serial) { }
    }
}
