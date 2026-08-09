using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bulbous violet corpse")]
    public sealed class SrdBulbousViolet : SrdMonster
    {
        [Constructable]
        public SrdBulbousViolet() : base("BulbousViolet") 
        {
        }

        public SrdBulbousViolet(Serial serial) : base(serial) { }
    }
}
