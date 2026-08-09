using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black sun priestess corpse")]
    public sealed class SrdBlackSunPriestess : SrdMonster
    {
        [Constructable]
        public SrdBlackSunPriestess() : base("BlackSunPriestess") 
        {
        }

        public SrdBlackSunPriestess(Serial serial) : base(serial) { }
    }
}
