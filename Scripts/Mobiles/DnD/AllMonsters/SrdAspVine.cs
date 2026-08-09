using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a asp vine corpse")]
    public sealed class SrdAspVine : SrdMonster
    {
        [Constructable]
        public SrdAspVine() : base("AspVine") 
        {
        }

        public SrdAspVine(Serial serial) : base(serial) { }
    }
}
