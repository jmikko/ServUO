using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep troll corpse")]
    public sealed class SrdDeepTroll : SrdMonster
    {
        [Constructable]
        public SrdDeepTroll() : base("DeepTroll") 
        {
        }

        public SrdDeepTroll(Serial serial) : base(serial) { }
    }
}
