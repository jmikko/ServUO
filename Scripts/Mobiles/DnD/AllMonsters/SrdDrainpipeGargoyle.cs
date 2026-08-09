using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drainpipe gargoyle corpse")]
    public sealed class SrdDrainpipeGargoyle : SrdMonster
    {
        [Constructable]
        public SrdDrainpipeGargoyle() : base("DrainpipeGargoyle") 
        {
        }

        public SrdDrainpipeGargoyle(Serial serial) : base(serial) { }
    }
}
