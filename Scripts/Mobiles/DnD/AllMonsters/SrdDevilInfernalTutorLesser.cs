using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil, infernal tutor, lesser corpse")]
    public sealed class SrdDevilInfernalTutorLesser : SrdMonster
    {
        [Constructable]
        public SrdDevilInfernalTutorLesser() : base("DevilInfernalTutorLesser") 
        {
        }

        public SrdDevilInfernalTutorLesser(Serial serial) : base(serial) { }
    }
}
