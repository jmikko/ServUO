using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aalpamac corpse")]
    public sealed class SrdAalpamac : SrdMonster
    {
        [Constructable]
        public SrdAalpamac() : base("Aalpamac") 
        {
        }

        public SrdAalpamac(Serial serial) : base(serial) { }
    }
}
