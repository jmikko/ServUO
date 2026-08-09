using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a azza gremlin corpse")]
    public sealed class SrdAzzaGremlin : SrdMonster
    {
        [Constructable]
        public SrdAzzaGremlin() : base("AzzaGremlin") 
        {
        }

        public SrdAzzaGremlin(Serial serial) : base(serial) { }
    }
}
