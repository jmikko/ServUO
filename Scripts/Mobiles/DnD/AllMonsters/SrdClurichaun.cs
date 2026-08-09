using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clurichaun corpse")]
    public sealed class SrdClurichaun : SrdMonster
    {
        [Constructable]
        public SrdClurichaun() : base("Clurichaun") 
        {
        }

        public SrdClurichaun(Serial serial) : base(serial) { }
    }
}
