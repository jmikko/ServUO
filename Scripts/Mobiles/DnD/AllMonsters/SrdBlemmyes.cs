using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blemmyes corpse")]
    public sealed class SrdBlemmyes : SrdMonster
    {
        [Constructable]
        public SrdBlemmyes() : base("Blemmyes") 
        {
        }

        public SrdBlemmyes(Serial serial) : base(serial) { }
    }
}
