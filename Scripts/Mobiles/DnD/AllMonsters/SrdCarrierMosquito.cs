using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a carrier mosquito corpse")]
    public sealed class SrdCarrierMosquito : SrdMonster
    {
        [Constructable]
        public SrdCarrierMosquito() : base("CarrierMosquito") 
        {
        }

        public SrdCarrierMosquito(Serial serial) : base(serial) { }
    }
}
