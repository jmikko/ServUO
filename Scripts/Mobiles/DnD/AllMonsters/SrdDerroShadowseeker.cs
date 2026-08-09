using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derro shadowseeker corpse")]
    public sealed class SrdDerroShadowseeker : SrdMonster
    {
        [Constructable]
        public SrdDerroShadowseeker() : base("DerroShadowseeker") 
        {
        }

        public SrdDerroShadowseeker(Serial serial) : base(serial) { }
    }
}
