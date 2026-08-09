using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dread examiner corpse")]
    public sealed class SrdDreadExaminer : SrdMonster
    {
        [Constructable]
        public SrdDreadExaminer() : base("DreadExaminer") 
        {
        }

        public SrdDreadExaminer(Serial serial) : base(serial) { }
    }
}
