using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cyclops corpse")]
    public sealed class SrdCyclops : SrdMonster
    {
        [Constructable]
        public SrdCyclops() : base("Cyclops") 
        {
        }

        public SrdCyclops(Serial serial) : base(serial) { }
    }
}
