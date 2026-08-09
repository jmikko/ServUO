using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a androsphinx corpse")]
    public sealed class SrdAndrosphinx : SrdMonster
    {
        [Constructable]
        public SrdAndrosphinx() : base("Androsphinx") 
        {
        }

        public SrdAndrosphinx(Serial serial) : base(serial) { }
    }
}
