using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arcamag corpse")]
    public sealed class SrdArcamag : SrdMonster
    {
        [Constructable]
        public SrdArcamag() : base("Arcamag") 
        {
        }

        public SrdArcamag(Serial serial) : base(serial) { }
    }
}
