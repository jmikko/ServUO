using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a akyishigal corpse")]
    public sealed class SrdAkyishigal : SrdMonster
    {
        [Constructable]
        public SrdAkyishigal() : base("Akyishigal") 
        {
        }

        public SrdAkyishigal(Serial serial) : base(serial) { }
    }
}
