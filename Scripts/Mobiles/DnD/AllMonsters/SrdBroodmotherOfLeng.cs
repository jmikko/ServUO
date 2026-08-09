using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a broodmother of leng corpse")]
    public sealed class SrdBroodmotherOfLeng : SrdMonster
    {
        [Constructable]
        public SrdBroodmotherOfLeng() : base("BroodmotherOfLeng") 
        {
        }

        public SrdBroodmotherOfLeng(Serial serial) : base(serial) { }
    }
}
