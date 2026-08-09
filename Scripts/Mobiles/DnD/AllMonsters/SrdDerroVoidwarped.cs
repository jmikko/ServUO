using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derro, voidwarped corpse")]
    public sealed class SrdDerroVoidwarped : SrdMonster
    {
        [Constructable]
        public SrdDerroVoidwarped() : base("DerroVoidwarped") 
        {
        }

        public SrdDerroVoidwarped(Serial serial) : base(serial) { }
    }
}
