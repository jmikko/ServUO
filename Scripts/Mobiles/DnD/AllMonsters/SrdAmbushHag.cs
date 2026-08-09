using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ambush hag corpse")]
    public sealed class SrdAmbushHag : SrdMonster
    {
        [Constructable]
        public SrdAmbushHag() : base("AmbushHag") 
        {
        }

        public SrdAmbushHag(Serial serial) : base(serial) { }
    }
}
