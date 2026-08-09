using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a archpriest corpse")]
    public sealed class SrdArchpriest : SrdMonster
    {
        [Constructable]
        public SrdArchpriest() : base("Archpriest") 
        {
        }

        public SrdArchpriest(Serial serial) : base(serial) { }
    }
}
