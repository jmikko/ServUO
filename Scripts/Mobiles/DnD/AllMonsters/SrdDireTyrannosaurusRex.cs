using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dire tyrannosaurus rex corpse")]
    public sealed class SrdDireTyrannosaurusRex : SrdMonster
    {
        [Constructable]
        public SrdDireTyrannosaurusRex() : base("DireTyrannosaurusRex") 
        {
        }

        public SrdDireTyrannosaurusRex(Serial serial) : base(serial) { }
    }
}
