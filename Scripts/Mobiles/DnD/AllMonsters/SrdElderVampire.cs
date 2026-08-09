using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a elder vampire corpse")]
    public sealed class SrdElderVampire : SrdMonster
    {
        [Constructable]
        public SrdElderVampire() : base("ElderVampire") 
        {
        }

        public SrdElderVampire(Serial serial) : base(serial) { }
    }
}
