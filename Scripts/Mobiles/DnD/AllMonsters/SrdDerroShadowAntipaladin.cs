using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derro shadow antipaladin corpse")]
    public sealed class SrdDerroShadowAntipaladin : SrdMonster
    {
        [Constructable]
        public SrdDerroShadowAntipaladin() : base("DerroShadowAntipaladin") 
        {
        }

        public SrdDerroShadowAntipaladin(Serial serial) : base(serial) { }
    }
}
