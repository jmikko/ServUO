using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blood ooze corpse")]
    public sealed class SrdBloodOoze : SrdMonster
    {
        [Constructable]
        public SrdBloodOoze() : base("BloodOoze") 
        {
        }

        public SrdBloodOoze(Serial serial) : base(serial) { }
    }
}
