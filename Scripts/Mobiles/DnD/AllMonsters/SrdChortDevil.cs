using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chort devil corpse")]
    public sealed class SrdChortDevil : SrdMonster
    {
        [Constructable]
        public SrdChortDevil() : base("ChortDevil") 
        {
        }

        public SrdChortDevil(Serial serial) : base(serial) { }
    }
}
