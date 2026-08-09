using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crypt spider corpse")]
    public sealed class SrdCryptSpider : SrdMonster
    {
        [Constructable]
        public SrdCryptSpider() : base("CryptSpider") 
        {
        }

        public SrdCryptSpider(Serial serial) : base(serial) { }
    }
}
