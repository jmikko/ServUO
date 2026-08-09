using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angel of judgment corpse")]
    public sealed class SrdAngelOfJudgment : SrdMonster
    {
        [Constructable]
        public SrdAngelOfJudgment() : base("AngelOfJudgment") 
        {
        }

        public SrdAngelOfJudgment(Serial serial) : base(serial) { }
    }
}
