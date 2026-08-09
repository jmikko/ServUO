using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon eel corpse")]
    public sealed class SrdDragonEel : SrdMonster
    {
        [Constructable]
        public SrdDragonEel() : base("DragonEel") 
        {
        }

        public SrdDragonEel(Serial serial) : base(serial) { }
    }
}
