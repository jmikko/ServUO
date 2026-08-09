using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aural hunter corpse")]
    public sealed class SrdAuralHunter : SrdMonster
    {
        [Constructable]
        public SrdAuralHunter() : base("AuralHunter") 
        {
        }

        public SrdAuralHunter(Serial serial) : base(serial) { }
    }
}
