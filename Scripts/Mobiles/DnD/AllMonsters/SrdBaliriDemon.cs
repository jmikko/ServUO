using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a baliri demon corpse")]
    public sealed class SrdBaliriDemon : SrdMonster
    {
        [Constructable]
        public SrdBaliriDemon() : base("BaliriDemon") 
        {
        }

        public SrdBaliriDemon(Serial serial) : base(serial) { }
    }
}
