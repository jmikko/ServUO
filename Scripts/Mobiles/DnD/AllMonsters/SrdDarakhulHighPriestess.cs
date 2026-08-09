using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a darakhul high priestess corpse")]
    public sealed class SrdDarakhulHighPriestess : SrdMonster
    {
        [Constructable]
        public SrdDarakhulHighPriestess() : base("DarakhulHighPriestess") 
        {
        }

        public SrdDarakhulHighPriestess(Serial serial) : base(serial) { }
    }
}
