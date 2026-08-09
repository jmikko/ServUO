using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chimera, royal corpse")]
    public sealed class SrdChimeraRoyal : SrdMonster
    {
        [Constructable]
        public SrdChimeraRoyal() : base("ChimeraRoyal") 
        {
        }

        public SrdChimeraRoyal(Serial serial) : base(serial) { }
    }
}
