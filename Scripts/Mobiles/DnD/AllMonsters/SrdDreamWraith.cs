using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dream wraith corpse")]
    public sealed class SrdDreamWraith : SrdMonster
    {
        [Constructable]
        public SrdDreamWraith() : base("DreamWraith") 
        {
        }

        public SrdDreamWraith(Serial serial) : base(serial) { }
    }
}
