using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a domovoi corpse")]
    public sealed class SrdDomovoi : SrdMonster
    {
        [Constructable]
        public SrdDomovoi() : base("Domovoi") 
        {
        }

        public SrdDomovoi(Serial serial) : base(serial) { }
    }
}
