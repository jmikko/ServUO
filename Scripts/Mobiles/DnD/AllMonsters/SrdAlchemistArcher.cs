using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alchemist archer corpse")]
    public sealed class SrdAlchemistArcher : SrdMonster
    {
        [Constructable]
        public SrdAlchemistArcher() : base("AlchemistArcher") 
        {
        }

        public SrdAlchemistArcher(Serial serial) : base(serial) { }
    }
}
