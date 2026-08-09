using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black knight commander corpse")]
    public sealed class SrdBlackKnightCommander : SrdMonster
    {
        [Constructable]
        public SrdBlackKnightCommander() : base("BlackKnightCommander") 
        {
        }

        public SrdBlackKnightCommander(Serial serial) : base(serial) { }
    }
}
