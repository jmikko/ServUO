using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ankylosaurus corpse")]
    public sealed class SrdAnkylosaurus : SrdMonster
    {
        [Constructable]
        public SrdAnkylosaurus() : base("Ankylosaurus") 
        {
        }

        public SrdAnkylosaurus(Serial serial) : base(serial) { }
    }
}
