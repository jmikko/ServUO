using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a akhlut corpse")]
    public sealed class SrdAkhlut : SrdMonster
    {
        [Constructable]
        public SrdAkhlut() : base("Akhlut") 
        {
        }

        public SrdAkhlut(Serial serial) : base(serial) { }
    }
}
