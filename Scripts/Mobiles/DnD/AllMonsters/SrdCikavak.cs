using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cikavak corpse")]
    public sealed class SrdCikavak : SrdMonster
    {
        [Constructable]
        public SrdCikavak() : base("Cikavak") 
        {
        }

        public SrdCikavak(Serial serial) : base(serial) { }
    }
}
