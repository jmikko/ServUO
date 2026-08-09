using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derro guard corpse")]
    public sealed class SrdDerroGuard : SrdMonster
    {
        [Constructable]
        public SrdDerroGuard() : base("DerroGuard") 
        {
        }

        public SrdDerroGuard(Serial serial) : base(serial) { }
    }
}
