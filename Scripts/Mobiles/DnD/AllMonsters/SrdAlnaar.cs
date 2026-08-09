using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alnaar corpse")]
    public sealed class SrdAlnaar : SrdMonster
    {
        [Constructable]
        public SrdAlnaar() : base("Alnaar") 
        {
        }

        public SrdAlnaar(Serial serial) : base(serial) { }
    }
}
