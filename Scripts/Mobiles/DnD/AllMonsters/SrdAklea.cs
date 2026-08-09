using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aklea corpse")]
    public sealed class SrdAklea : SrdMonster
    {
        [Constructable]
        public SrdAklea() : base("Aklea") 
        {
        }

        public SrdAklea(Serial serial) : base(serial) { }
    }
}
