using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crocodile corpse")]
    public sealed class SrdCrocodile : SrdMonster
    {
        [Constructable]
        public SrdCrocodile() : base("Crocodile") 
        {
        }

        public SrdCrocodile(Serial serial) : base(serial) { }
    }
}
