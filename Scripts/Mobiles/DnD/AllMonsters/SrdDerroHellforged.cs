using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derro, hellforged corpse")]
    public sealed class SrdDerroHellforged : SrdMonster
    {
        [Constructable]
        public SrdDerroHellforged() : base("DerroHellforged") 
        {
        }

        public SrdDerroHellforged(Serial serial) : base(serial) { }
    }
}
