using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a darakhul captain corpse")]
    public sealed class SrdDarakhulCaptain : SrdMonster
    {
        [Constructable]
        public SrdDarakhulCaptain() : base("DarakhulCaptain") 
        {
        }

        public SrdDarakhulCaptain(Serial serial) : base(serial) { }
    }
}
