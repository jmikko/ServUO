using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crocotta corpse")]
    public sealed class SrdCrocotta : SrdMonster
    {
        [Constructable]
        public SrdCrocotta() : base("Crocotta") 
        {
        }

        public SrdCrocotta(Serial serial) : base(serial) { }
    }
}
