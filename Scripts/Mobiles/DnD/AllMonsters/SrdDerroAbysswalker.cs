using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derro, abysswalker corpse")]
    public sealed class SrdDerroAbysswalker : SrdMonster
    {
        [Constructable]
        public SrdDerroAbysswalker() : base("DerroAbysswalker") 
        {
        }

        public SrdDerroAbysswalker(Serial serial) : base(serial) { }
    }
}
