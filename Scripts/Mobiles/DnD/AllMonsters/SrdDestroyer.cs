using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a destroyer corpse")]
    public sealed class SrdDestroyer : SrdMonster
    {
        [Constructable]
        public SrdDestroyer() : base("Destroyer") 
        {
        }

        public SrdDestroyer(Serial serial) : base(serial) { }
    }
}
