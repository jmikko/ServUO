using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a eel hound corpse")]
    public sealed class SrdEelHound : SrdMonster
    {
        [Constructable]
        public SrdEelHound() : base("EelHound") 
        {
        }

        public SrdEelHound(Serial serial) : base(serial) { }
    }
}
