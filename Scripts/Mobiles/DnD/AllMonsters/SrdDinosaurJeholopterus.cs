using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dinosaur, jeholopterus corpse")]
    public sealed class SrdDinosaurJeholopterus : SrdMonster
    {
        [Constructable]
        public SrdDinosaurJeholopterus() : base("DinosaurJeholopterus") 
        {
        }

        public SrdDinosaurJeholopterus(Serial serial) : base(serial) { }
    }
}
