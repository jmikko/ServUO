using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chronalmental corpse")]
    public sealed class SrdChronalmental : SrdMonster
    {
        [Constructable]
        public SrdChronalmental() : base("Chronalmental") 
        {
        }

        public SrdChronalmental(Serial serial) : base(serial) { }
    }
}
