using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil, devilflame juggler corpse")]
    public sealed class SrdDevilDevilflameJuggler : SrdMonster
    {
        [Constructable]
        public SrdDevilDevilflameJuggler() : base("DevilDevilflameJuggler") 
        {
        }

        public SrdDevilDevilflameJuggler(Serial serial) : base(serial) { }
    }
}
