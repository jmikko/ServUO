using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cloud giant noble corpse")]
    public sealed class SrdCloudGiantNoble : SrdMonster
    {
        [Constructable]
        public SrdCloudGiantNoble() : base("CloudGiantNoble") 
        {
        }

        public SrdCloudGiantNoble(Serial serial) : base(serial) { }
    }
}
