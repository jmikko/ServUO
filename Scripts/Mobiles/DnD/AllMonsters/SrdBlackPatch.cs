using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black patch corpse")]
    public sealed class SrdBlackPatch : SrdMonster
    {
        [Constructable]
        public SrdBlackPatch() : base("BlackPatch") 
        {
        }

        public SrdBlackPatch(Serial serial) : base(serial) { }
    }
}
