using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a avestruzii champion corpse")]
    public sealed class SrdAvestruziiChampion : SrdMonster
    {
        [Constructable]
        public SrdAvestruziiChampion() : base("AvestruziiChampion") 
        {
        }

        public SrdAvestruziiChampion(Serial serial) : base(serial) { }
    }
}
