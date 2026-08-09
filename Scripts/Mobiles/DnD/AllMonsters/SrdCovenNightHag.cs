using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a coven night hag corpse")]
    public sealed class SrdCovenNightHag : SrdMonster
    {
        [Constructable]
        public SrdCovenNightHag() : base("CovenNightHag") 
        {
        }

        public SrdCovenNightHag(Serial serial) : base(serial) { }
    }
}
