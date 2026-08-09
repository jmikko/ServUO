using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil, infernal tutor corpse")]
    public sealed class SrdDevilInfernalTutor : SrdMonster
    {
        [Constructable]
        public SrdDevilInfernalTutor() : base("DevilInfernalTutor") 
        {
        }

        public SrdDevilInfernalTutor(Serial serial) : base(serial) { }
    }
}
