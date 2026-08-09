using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dracotaur corpse")]
    public sealed class SrdDracotaur : SrdMonster
    {
        [Constructable]
        public SrdDracotaur() : base("Dracotaur") 
        {
        }

        public SrdDracotaur(Serial serial) : base(serial) { }
    }
}
