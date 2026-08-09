using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arcanaphage corpse")]
    public sealed class SrdArcanaphage : SrdMonster
    {
        [Constructable]
        public SrdArcanaphage() : base("Arcanaphage") 
        {
        }

        public SrdArcanaphage(Serial serial) : base(serial) { }
    }
}
