using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deathwisp corpse")]
    public sealed class SrdDeathwisp : SrdMonster
    {
        [Constructable]
        public SrdDeathwisp() : base("Deathwisp") 
        {
        }

        public SrdDeathwisp(Serial serial) : base(serial) { }
    }
}
