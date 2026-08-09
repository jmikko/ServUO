using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chemosit corpse")]
    public sealed class SrdChemosit : SrdMonster
    {
        [Constructable]
        public SrdChemosit() : base("Chemosit") 
        {
        }

        public SrdChemosit(Serial serial) : base(serial) { }
    }
}
