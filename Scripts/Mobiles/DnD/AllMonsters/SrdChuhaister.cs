using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chuhaister corpse")]
    public sealed class SrdChuhaister : SrdMonster
    {
        [Constructable]
        public SrdChuhaister() : base("Chuhaister") 
        {
        }

        public SrdChuhaister(Serial serial) : base(serial) { }
    }
}
