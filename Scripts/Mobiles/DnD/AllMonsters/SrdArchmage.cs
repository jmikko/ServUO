using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a archmage corpse")]
    public sealed class SrdArchmage : SrdMonster
    {
        [Constructable]
        public SrdArchmage() : base("Archmage") 
        {
        }

        public SrdArchmage(Serial serial) : base(serial) { }
    }
}
