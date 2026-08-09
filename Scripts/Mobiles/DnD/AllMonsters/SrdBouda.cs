using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bouda corpse")]
    public sealed class SrdBouda : SrdMonster
    {
        [Constructable]
        public SrdBouda() : base("Bouda") 
        {
        }

        public SrdBouda(Serial serial) : base(serial) { }
    }
}
