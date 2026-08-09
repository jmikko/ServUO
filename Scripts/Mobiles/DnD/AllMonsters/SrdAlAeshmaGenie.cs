using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a al-aeshma genie corpse")]
    public sealed class SrdAlAeshmaGenie : SrdMonster
    {
        [Constructable]
        public SrdAlAeshmaGenie() : base("AlAeshmaGenie") 
        {
        }

        public SrdAlAeshmaGenie(Serial serial) : base(serial) { }
    }
}
