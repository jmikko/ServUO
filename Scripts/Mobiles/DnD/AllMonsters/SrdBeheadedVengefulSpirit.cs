using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a beheaded vengeful spirit corpse")]
    public sealed class SrdBeheadedVengefulSpirit : SrdMonster
    {
        [Constructable]
        public SrdBeheadedVengefulSpirit() : base("BeheadedVengefulSpirit") 
        {
        }

        public SrdBeheadedVengefulSpirit(Serial serial) : base(serial) { }
    }
}
