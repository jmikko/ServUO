using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a darakhul shadowmancer corpse")]
    public sealed class SrdDarakhulShadowmancer : SrdMonster
    {
        [Constructable]
        public SrdDarakhulShadowmancer() : base("DarakhulShadowmancer") 
        {
        }

        public SrdDarakhulShadowmancer(Serial serial) : base(serial) { }
    }
}
