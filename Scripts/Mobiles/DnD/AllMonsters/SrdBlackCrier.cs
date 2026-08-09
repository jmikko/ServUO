using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black crier corpse")]
    public sealed class SrdBlackCrier : SrdMonster
    {
        [Constructable]
        public SrdBlackCrier() : base("BlackCrier") 
        {
        }

        public SrdBlackCrier(Serial serial) : base(serial) { }
    }
}
