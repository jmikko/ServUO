using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arcane leviathan corpse")]
    public sealed class SrdArcaneLeviathan : SrdMonster
    {
        [Constructable]
        public SrdArcaneLeviathan() : base("ArcaneLeviathan") 
        {
        }

        public SrdArcaneLeviathan(Serial serial) : base(serial) { }
    }
}
