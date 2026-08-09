using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult brass dragon corpse")]
    public sealed class SrdAdultBrassDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultBrassDragon() : base("AdultBrassDragon") 
        {
        }

        public SrdAdultBrassDragon(Serial serial) : base(serial) { }
    }
}
