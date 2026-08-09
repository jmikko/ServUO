using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alabroza corpse")]
    public sealed class SrdAlabroza : SrdMonster
    {
        [Constructable]
        public SrdAlabroza() : base("Alabroza") 
        {
        }

        public SrdAlabroza(Serial serial) : base(serial) { }
    }
}
