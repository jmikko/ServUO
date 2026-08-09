using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a despair and anger corpse")]
    public sealed class SrdDespairAndAnger : SrdMonster
    {
        [Constructable]
        public SrdDespairAndAnger() : base("DespairAndAnger") 
        {
        }

        public SrdDespairAndAnger(Serial serial) : base(serial) { }
    }
}
