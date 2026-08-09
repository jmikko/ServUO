using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a daeodon corpse")]
    public sealed class SrdDaeodon : SrdMonster
    {
        [Constructable]
        public SrdDaeodon() : base("Daeodon") 
        {
        }

        public SrdDaeodon(Serial serial) : base(serial) { }
    }
}
