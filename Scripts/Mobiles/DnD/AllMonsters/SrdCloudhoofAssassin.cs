using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cloudhoof assassin corpse")]
    public sealed class SrdCloudhoofAssassin : SrdMonster
    {
        [Constructable]
        public SrdCloudhoofAssassin() : base("CloudhoofAssassin") 
        {
        }

        public SrdCloudhoofAssassin(Serial serial) : base(serial) { }
    }
}
