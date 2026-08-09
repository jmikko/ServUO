using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aniwye corpse")]
    public sealed class SrdAniwye : SrdMonster
    {
        [Constructable]
        public SrdAniwye() : base("Aniwye") 
        {
        }

        public SrdAniwye(Serial serial) : base(serial) { }
    }
}
