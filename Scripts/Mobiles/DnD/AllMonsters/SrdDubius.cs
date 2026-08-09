using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dubius corpse")]
    public sealed class SrdDubius : SrdMonster
    {
        [Constructable]
        public SrdDubius() : base("Dubius") 
        {
        }

        public SrdDubius(Serial serial) : base(serial) { }
    }
}
