using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blood flurry corpse")]
    public sealed class SrdBloodFlurry : SrdMonster
    {
        [Constructable]
        public SrdBloodFlurry() : base("BloodFlurry") 
        {
        }

        public SrdBloodFlurry(Serial serial) : base(serial) { }
    }
}
