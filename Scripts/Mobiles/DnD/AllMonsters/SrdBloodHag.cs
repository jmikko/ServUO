using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blood hag corpse")]
    public sealed class SrdBloodHag : SrdMonster
    {
        [Constructable]
        public SrdBloodHag() : base("BloodHag") 
        {
        }

        public SrdBloodHag(Serial serial) : base(serial) { }
    }
}
