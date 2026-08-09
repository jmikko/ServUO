using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a apau perape corpse")]
    public sealed class SrdApauPerape : SrdMonster
    {
        [Constructable]
        public SrdApauPerape() : base("ApauPerape") 
        {
        }

        public SrdApauPerape(Serial serial) : base(serial) { }
    }
}
