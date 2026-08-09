using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dire centipede corpse")]
    public sealed class SrdDireCentipede : SrdMonster
    {
        [Constructable]
        public SrdDireCentipede() : base("DireCentipede") 
        {
        }

        public SrdDireCentipede(Serial serial) : base(serial) { }
    }
}
