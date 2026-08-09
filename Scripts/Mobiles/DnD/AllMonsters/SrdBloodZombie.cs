using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blood zombie corpse")]
    public sealed class SrdBloodZombie : SrdMonster
    {
        [Constructable]
        public SrdBloodZombie() : base("BloodZombie") 
        {
        }

        public SrdBloodZombie(Serial serial) : base(serial) { }
    }
}
