using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a eleinomae corpse")]
    public sealed class SrdEleinomae : SrdMonster
    {
        [Constructable]
        public SrdEleinomae() : base("Eleinomae") 
        {
        }

        public SrdEleinomae(Serial serial) : base(serial) { }
    }
}
