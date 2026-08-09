using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a egret harpy corpse")]
    public sealed class SrdEgretHarpy : SrdMonster
    {
        [Constructable]
        public SrdEgretHarpy() : base("EgretHarpy") 
        {
        }

        public SrdEgretHarpy(Serial serial) : base(serial) { }
    }
}
