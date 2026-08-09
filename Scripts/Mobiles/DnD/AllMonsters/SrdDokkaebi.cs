using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dokkaebi corpse")]
    public sealed class SrdDokkaebi : SrdMonster
    {
        [Constructable]
        public SrdDokkaebi() : base("Dokkaebi") 
        {
        }

        public SrdDokkaebi(Serial serial) : base(serial) { }
    }
}
