using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a accursed defiler corpse")]
    public sealed class SrdAccursedDefiler : SrdMonster
    {
        [Constructable]
        public SrdAccursedDefiler() : base("AccursedDefiler") 
        {
        }

        public SrdAccursedDefiler(Serial serial) : base(serial) { }
    }
}
