using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a beggar ghoul corpse")]
    public sealed class SrdBeggarGhoul : SrdMonster
    {
        [Constructable]
        public SrdBeggarGhoul() : base("BeggarGhoul") 
        {
        }

        public SrdBeggarGhoul(Serial serial) : base(serial) { }
    }
}
