using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dark servant corpse")]
    public sealed class SrdDarkServant : SrdMonster
    {
        [Constructable]
        public SrdDarkServant() : base("DarkServant") 
        {
        }

        public SrdDarkServant(Serial serial) : base(serial) { }
    }
}
