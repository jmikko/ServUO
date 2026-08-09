using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient mandriano corpse")]
    public sealed class SrdAncientMandriano : SrdMonster
    {
        [Constructable]
        public SrdAncientMandriano() : base("AncientMandriano") 
        {
        }

        public SrdAncientMandriano(Serial serial) : base(serial) { }
    }
}
