using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep gnome scout corpse")]
    public sealed class SrdDeepGnomeScout : SrdMonster
    {
        [Constructable]
        public SrdDeepGnomeScout() : base("DeepGnomeScout") 
        {
        }

        public SrdDeepGnomeScout(Serial serial) : base(serial) { }
    }
}
