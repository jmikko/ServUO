using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a edimmu corpse")]
    public sealed class SrdEdimmu : SrdMonster
    {
        [Constructable]
        public SrdEdimmu() : base("Edimmu") 
        {
        }

        public SrdEdimmu(Serial serial) : base(serial) { }
    }
}
