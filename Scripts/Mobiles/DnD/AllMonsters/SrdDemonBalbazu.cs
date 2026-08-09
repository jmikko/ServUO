using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a demon, balbazu corpse")]
    public sealed class SrdDemonBalbazu : SrdMonster
    {
        [Constructable]
        public SrdDemonBalbazu() : base("DemonBalbazu") 
        {
        }

        public SrdDemonBalbazu(Serial serial) : base(serial) { }
    }
}
