using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a eater of dust (yakat-shi) corpse")]
    public sealed class SrdEaterOfDustYakatShi : SrdMonster
    {
        [Constructable]
        public SrdEaterOfDustYakatShi() : base("EaterOfDustYakatShi") 
        {
        }

        public SrdEaterOfDustYakatShi(Serial serial) : base(serial) { }
    }
}
