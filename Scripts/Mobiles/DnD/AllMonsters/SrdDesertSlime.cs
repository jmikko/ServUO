using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a desert slime corpse")]
    public sealed class SrdDesertSlime : SrdMonster
    {
        [Constructable]
        public SrdDesertSlime() : base("DesertSlime") 
        {
        }

        public SrdDesertSlime(Serial serial) : base(serial) { }
    }
}
