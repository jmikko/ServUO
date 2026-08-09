using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cyclops myrmidon corpse")]
    public sealed class SrdCyclopsMyrmidon : SrdMonster
    {
        [Constructable]
        public SrdCyclopsMyrmidon() : base("CyclopsMyrmidon") 
        {
        }

        public SrdCyclopsMyrmidon(Serial serial) : base(serial) { }
    }
}
