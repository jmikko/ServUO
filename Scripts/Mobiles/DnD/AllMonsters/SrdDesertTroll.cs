using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a desert troll corpse")]
    public sealed class SrdDesertTroll : SrdMonster
    {
        [Constructable]
        public SrdDesertTroll() : base("DesertTroll") 
        {
        }

        public SrdDesertTroll(Serial serial) : base(serial) { }
    }
}
