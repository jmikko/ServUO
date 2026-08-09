using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a elder ghost boar corpse")]
    public sealed class SrdElderGhostBoar : SrdMonster
    {
        [Constructable]
        public SrdElderGhostBoar() : base("ElderGhostBoar") 
        {
        }

        public SrdElderGhostBoar(Serial serial) : base(serial) { }
    }
}
