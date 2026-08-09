using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boggard bravo corpse")]
    public sealed class SrdBoggardBravo : SrdMonster
    {
        [Constructable]
        public SrdBoggardBravo() : base("BoggardBravo") 
        {
        }

        public SrdBoggardBravo(Serial serial) : base(serial) { }
    }
}
