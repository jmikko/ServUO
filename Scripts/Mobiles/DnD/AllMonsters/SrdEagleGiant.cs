using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a eagle, giant corpse")]
    public sealed class SrdEagleGiant : SrdMonster
    {
        [Constructable]
        public SrdEagleGiant() : base("EagleGiant") 
        {
        }

        public SrdEagleGiant(Serial serial) : base(serial) { }
    }
}
