using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aviere corpse")]
    public sealed class SrdAviere : SrdMonster
    {
        [Constructable]
        public SrdAviere() : base("Aviere") 
        {
        }

        public SrdAviere(Serial serial) : base(serial) { }
    }
}
