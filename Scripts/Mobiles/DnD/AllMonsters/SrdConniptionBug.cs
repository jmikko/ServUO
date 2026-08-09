using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a conniption bug corpse")]
    public sealed class SrdConniptionBug : SrdMonster
    {
        [Constructable]
        public SrdConniptionBug() : base("ConniptionBug") 
        {
        }

        public SrdConniptionBug(Serial serial) : base(serial) { }
    }
}
