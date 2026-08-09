using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alp corpse")]
    public sealed class SrdAlp : SrdMonster
    {
        [Constructable]
        public SrdAlp() : base("Alp") 
        {
        }

        public SrdAlp(Serial serial) : base(serial) { }
    }
}
