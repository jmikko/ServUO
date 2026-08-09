using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a andrenjinyi corpse")]
    public sealed class SrdAndrenjinyi : SrdMonster
    {
        [Constructable]
        public SrdAndrenjinyi() : base("Andrenjinyi") 
        {
        }

        public SrdAndrenjinyi(Serial serial) : base(serial) { }
    }
}
