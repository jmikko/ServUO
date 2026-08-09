using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a desert giant corpse")]
    public sealed class SrdDesertGiant : SrdMonster
    {
        [Constructable]
        public SrdDesertGiant() : base("DesertGiant") 
        {
        }

        public SrdDesertGiant(Serial serial) : base(serial) { }
    }
}
