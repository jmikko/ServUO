using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alliumite corpse")]
    public sealed class SrdAlliumite : SrdMonster
    {
        [Constructable]
        public SrdAlliumite() : base("Alliumite") 
        {
        }

        public SrdAlliumite(Serial serial) : base(serial) { }
    }
}
