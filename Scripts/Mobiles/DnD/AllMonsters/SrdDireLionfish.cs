using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dire lionfish corpse")]
    public sealed class SrdDireLionfish : SrdMonster
    {
        [Constructable]
        public SrdDireLionfish() : base("DireLionfish") 
        {
        }

        public SrdDireLionfish(Serial serial) : base(serial) { }
    }
}
