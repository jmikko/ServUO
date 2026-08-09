using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devilbound gnome corpse")]
    public sealed class SrdDevilboundGnome : SrdMonster
    {
        [Constructable]
        public SrdDevilboundGnome() : base("DevilboundGnome") 
        {
        }

        public SrdDevilboundGnome(Serial serial) : base(serial) { }
    }
}
