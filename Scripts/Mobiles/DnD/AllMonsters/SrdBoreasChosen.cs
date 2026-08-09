using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boreas’ chosen corpse")]
    public sealed class SrdBoreasChosen : SrdMonster
    {
        [Constructable]
        public SrdBoreasChosen() : base("BoreasChosen") 
        {
        }

        public SrdBoreasChosen(Serial serial) : base(serial) { }
    }
}
