using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boloti corpse")]
    public sealed class SrdBoloti : SrdMonster
    {
        [Constructable]
        public SrdBoloti() : base("Boloti") 
        {
        }

        public SrdBoloti(Serial serial) : base(serial) { }
    }
}
