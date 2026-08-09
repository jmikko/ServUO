using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cyonaxin corpse")]
    public sealed class SrdCyonaxin : SrdMonster
    {
        [Constructable]
        public SrdCyonaxin() : base("Cyonaxin") 
        {
        }

        public SrdCyonaxin(Serial serial) : base(serial) { }
    }
}
