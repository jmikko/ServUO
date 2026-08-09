using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arbeyach corpse")]
    public sealed class SrdArbeyach : SrdMonster
    {
        [Constructable]
        public SrdArbeyach() : base("Arbeyach") 
        {
        }

        public SrdArbeyach(Serial serial) : base(serial) { }
    }
}
