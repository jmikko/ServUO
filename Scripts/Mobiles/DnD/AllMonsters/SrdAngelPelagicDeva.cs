using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angel, pelagic deva corpse")]
    public sealed class SrdAngelPelagicDeva : SrdMonster
    {
        [Constructable]
        public SrdAngelPelagicDeva() : base("AngelPelagicDeva") 
        {
        }

        public SrdAngelPelagicDeva(Serial serial) : base(serial) { }
    }
}
