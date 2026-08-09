using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a edjet initiate corpse")]
    public sealed class SrdEdjetInitiate : SrdMonster
    {
        [Constructable]
        public SrdEdjetInitiate() : base("EdjetInitiate") 
        {
        }

        public SrdEdjetInitiate(Serial serial) : base(serial) { }
    }
}
