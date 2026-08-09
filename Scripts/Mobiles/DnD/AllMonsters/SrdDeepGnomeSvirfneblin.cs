using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep gnome (svirfneblin) corpse")]
    public sealed class SrdDeepGnomeSvirfneblin : SrdMonster
    {
        [Constructable]
        public SrdDeepGnomeSvirfneblin() : base("DeepGnomeSvirfneblin") 
        {
        }

        public SrdDeepGnomeSvirfneblin(Serial serial) : base(serial) { }
    }
}
