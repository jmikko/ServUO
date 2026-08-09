using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chernomoi corpse")]
    public sealed class SrdChernomoi : SrdMonster
    {
        [Constructable]
        public SrdChernomoi() : base("Chernomoi") 
        {
        }

        public SrdChernomoi(Serial serial) : base(serial) { }
    }
}
