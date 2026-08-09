using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a akanka corpse")]
    public sealed class SrdAkanka : SrdMonster
    {
        [Constructable]
        public SrdAkanka() : base("Akanka") 
        {
        }

        public SrdAkanka(Serial serial) : base(serial) { }
    }
}
