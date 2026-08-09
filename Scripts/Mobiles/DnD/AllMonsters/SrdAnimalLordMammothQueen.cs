using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a animal lord, mammoth queen corpse")]
    public sealed class SrdAnimalLordMammothQueen : SrdMonster
    {
        [Constructable]
        public SrdAnimalLordMammothQueen() : base("AnimalLordMammothQueen") 
        {
        }

        public SrdAnimalLordMammothQueen(Serial serial) : base(serial) { }
    }
}
