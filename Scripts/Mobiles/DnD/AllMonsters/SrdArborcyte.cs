using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arborcyte corpse")]
    public sealed class SrdArborcyte : SrdMonster
    {
        [Constructable]
        public SrdArborcyte() : base("Arborcyte") 
        {
        }

        public SrdArborcyte(Serial serial) : base(serial) { }
    }
}
