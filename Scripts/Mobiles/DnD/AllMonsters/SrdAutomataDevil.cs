using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a automata devil corpse")]
    public sealed class SrdAutomataDevil : SrdMonster
    {
        [Constructable]
        public SrdAutomataDevil() : base("AutomataDevil") 
        {
        }

        public SrdAutomataDevil(Serial serial) : base(serial) { }
    }
}
