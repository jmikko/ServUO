using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a behir magus corpse")]
    public sealed class SrdBehirMagus : SrdMonster
    {
        [Constructable]
        public SrdBehirMagus() : base("BehirMagus") 
        {
        }

        public SrdBehirMagus(Serial serial) : base(serial) { }
    }
}
