using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alpha werewolf corpse")]
    public sealed class SrdAlphaWerewolf : SrdMonster
    {
        [Constructable]
        public SrdAlphaWerewolf() : base("AlphaWerewolf") 
        {
        }

        public SrdAlphaWerewolf(Serial serial) : base(serial) { }
    }
}
