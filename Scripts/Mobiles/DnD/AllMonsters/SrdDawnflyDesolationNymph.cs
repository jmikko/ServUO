using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dawnfly, desolation nymph corpse")]
    public sealed class SrdDawnflyDesolationNymph : SrdMonster
    {
        [Constructable]
        public SrdDawnflyDesolationNymph() : base("DawnflyDesolationNymph") 
        {
        }

        public SrdDawnflyDesolationNymph(Serial serial) : base(serial) { }
    }
}
