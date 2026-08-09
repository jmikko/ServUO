using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angatra corpse")]
    public sealed class SrdAngatra : SrdMonster
    {
        [Constructable]
        public SrdAngatra() : base("Angatra") 
        {
        }

        public SrdAngatra(Serial serial) : base(serial) { }
    }
}
