using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a apprentice mage corpse")]
    public sealed class SrdApprenticeMage : SrdMonster
    {
        [Constructable]
        public SrdApprenticeMage() : base("ApprenticeMage") 
        {
        }

        public SrdApprenticeMage(Serial serial) : base(serial) { }
    }
}
