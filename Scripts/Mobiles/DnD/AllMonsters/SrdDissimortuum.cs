using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dissimortuum corpse")]
    public sealed class SrdDissimortuum : SrdMonster
    {
        [Constructable]
        public SrdDissimortuum() : base("Dissimortuum") 
        {
        }

        public SrdDissimortuum(Serial serial) : base(serial) { }
    }
}
