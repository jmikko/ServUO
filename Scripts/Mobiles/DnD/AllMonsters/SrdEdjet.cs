using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a edjet corpse")]
    public sealed class SrdEdjet : SrdMonster
    {
        [Constructable]
        public SrdEdjet() : base("Edjet") 
        {
        }

        public SrdEdjet(Serial serial) : base(serial) { }
    }
}
