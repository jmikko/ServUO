using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a darakhul corpse")]
    public sealed class SrdDarakhul : SrdMonster
    {
        [Constructable]
        public SrdDarakhul() : base("Darakhul") 
        {
        }

        public SrdDarakhul(Serial serial) : base(serial) { }
    }
}
