using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boot grabber corpse")]
    public sealed class SrdBootGrabber : SrdMonster
    {
        [Constructable]
        public SrdBootGrabber() : base("BootGrabber") 
        {
        }

        public SrdBootGrabber(Serial serial) : base(serial) { }
    }
}
