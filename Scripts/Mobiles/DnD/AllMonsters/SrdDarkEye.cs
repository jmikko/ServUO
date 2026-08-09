using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dark eye corpse")]
    public sealed class SrdDarkEye : SrdMonster
    {
        [Constructable]
        public SrdDarkEye() : base("DarkEye") 
        {
        }

        public SrdDarkEye(Serial serial) : base(serial) { }
    }
}
