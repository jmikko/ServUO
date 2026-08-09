using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep gnome corpse")]
    public sealed class SrdDeepGnome : SrdMonster
    {
        [Constructable]
        public SrdDeepGnome() : base("DeepGnome") 
        {
        }

        public SrdDeepGnome(Serial serial) : base(serial) { }
    }
}
