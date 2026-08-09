using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a attercroppe corpse")]
    public sealed class SrdAttercroppe : SrdMonster
    {
        [Constructable]
        public SrdAttercroppe() : base("Attercroppe") 
        {
        }

        public SrdAttercroppe(Serial serial) : base(serial) { }
    }
}
