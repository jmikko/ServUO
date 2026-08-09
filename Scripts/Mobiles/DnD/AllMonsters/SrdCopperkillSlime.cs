using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a copperkill slime corpse")]
    public sealed class SrdCopperkillSlime : SrdMonster
    {
        [Constructable]
        public SrdCopperkillSlime() : base("CopperkillSlime") 
        {
        }

        public SrdCopperkillSlime(Serial serial) : base(serial) { }
    }
}
