using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crusher corpse")]
    public sealed class SrdCrusher : SrdMonster
    {
        [Constructable]
        public SrdCrusher() : base("Crusher") 
        {
        }

        public SrdCrusher(Serial serial) : base(serial) { }
    }
}
