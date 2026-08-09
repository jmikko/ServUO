using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black sun orc corpse")]
    public sealed class SrdBlackSunOrc : SrdMonster
    {
        [Constructable]
        public SrdBlackSunOrc() : base("BlackSunOrc") 
        {
        }

        public SrdBlackSunOrc(Serial serial) : base(serial) { }
    }
}
