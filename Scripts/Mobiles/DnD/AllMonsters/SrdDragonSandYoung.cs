using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon, sand young corpse")]
    public sealed class SrdDragonSandYoung : SrdMonster
    {
        [Constructable]
        public SrdDragonSandYoung() : base("DragonSandYoung") 
        {
        }

        public SrdDragonSandYoung(Serial serial) : base(serial) { }
    }
}
