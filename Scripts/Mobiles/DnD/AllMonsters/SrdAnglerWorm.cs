using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angler worm corpse")]
    public sealed class SrdAnglerWorm : SrdMonster
    {
        [Constructable]
        public SrdAnglerWorm() : base("AnglerWorm") 
        {
        }

        public SrdAnglerWorm(Serial serial) : base(serial) { }
    }
}
