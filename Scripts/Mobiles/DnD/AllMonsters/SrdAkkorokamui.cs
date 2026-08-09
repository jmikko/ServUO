using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a akkorokamui corpse")]
    public sealed class SrdAkkorokamui : SrdMonster
    {
        [Constructable]
        public SrdAkkorokamui() : base("Akkorokamui") 
        {
        }

        public SrdAkkorokamui(Serial serial) : base(serial) { }
    }
}
