using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aurora horribilis corpse")]
    public sealed class SrdAuroraHorribilis : SrdMonster
    {
        [Constructable]
        public SrdAuroraHorribilis() : base("AuroraHorribilis") 
        {
        }

        public SrdAuroraHorribilis(Serial serial) : base(serial) { }
    }
}
