using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a elder black pudding corpse")]
    public sealed class SrdElderBlackPudding : SrdMonster
    {
        [Constructable]
        public SrdElderBlackPudding() : base("ElderBlackPudding") 
        {
        }

        public SrdElderBlackPudding(Serial serial) : base(serial) { }
    }
}
