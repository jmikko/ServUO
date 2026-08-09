using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chill haunt corpse")]
    public sealed class SrdChillHaunt : SrdMonster
    {
        [Constructable]
        public SrdChillHaunt() : base("ChillHaunt") 
        {
        }

        public SrdChillHaunt(Serial serial) : base(serial) { }
    }
}
