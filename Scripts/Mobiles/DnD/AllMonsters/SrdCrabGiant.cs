using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crab, giant corpse")]
    public sealed class SrdCrabGiant : SrdMonster
    {
        [Constructable]
        public SrdCrabGiant() : base("CrabGiant") 
        {
        }

        public SrdCrabGiant(Serial serial) : base(serial) { }
    }
}
