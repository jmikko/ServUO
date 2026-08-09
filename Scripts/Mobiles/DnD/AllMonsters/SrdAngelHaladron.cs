using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angel, haladron corpse")]
    public sealed class SrdAngelHaladron : SrdMonster
    {
        [Constructable]
        public SrdAngelHaladron() : base("AngelHaladron") 
        {
        }

        public SrdAngelHaladron(Serial serial) : base(serial) { }
    }
}
