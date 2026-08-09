using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ankou soul herald corpse")]
    public sealed class SrdAnkouSoulHerald : SrdMonster
    {
        [Constructable]
        public SrdAnkouSoulHerald() : base("AnkouSoulHerald") 
        {
        }

        public SrdAnkouSoulHerald(Serial serial) : base(serial) { }
    }
}
