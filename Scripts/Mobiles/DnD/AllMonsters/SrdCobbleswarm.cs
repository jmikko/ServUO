using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cobbleswarm corpse")]
    public sealed class SrdCobbleswarm : SrdMonster
    {
        [Constructable]
        public SrdCobbleswarm() : base("Cobbleswarm") 
        {
        }

        public SrdCobbleswarm(Serial serial) : base(serial) { }
    }
}
