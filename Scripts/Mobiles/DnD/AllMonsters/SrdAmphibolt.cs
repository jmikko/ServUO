using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a amphibolt corpse")]
    public sealed class SrdAmphibolt : SrdMonster
    {
        [Constructable]
        public SrdAmphibolt() : base("Amphibolt") 
        {
        }

        public SrdAmphibolt(Serial serial) : base(serial) { }
    }
}
