using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dire pangolin corpse")]
    public sealed class SrdDirePangolin : SrdMonster
    {
        [Constructable]
        public SrdDirePangolin() : base("DirePangolin") 
        {
        }

        public SrdDirePangolin(Serial serial) : base(serial) { }
    }
}
