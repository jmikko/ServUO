using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a balloon spider corpse")]
    public sealed class SrdBalloonSpider : SrdMonster
    {
        [Constructable]
        public SrdBalloonSpider() : base("BalloonSpider") 
        {
        }

        public SrdBalloonSpider(Serial serial) : base(serial) { }
    }
}
