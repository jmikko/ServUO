using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a darakhul spy corpse")]
    public sealed class SrdDarakhulSpy : SrdMonster
    {
        [Constructable]
        public SrdDarakhulSpy() : base("DarakhulSpy") 
        {
        }

        public SrdDarakhulSpy(Serial serial) : base(serial) { }
    }
}
