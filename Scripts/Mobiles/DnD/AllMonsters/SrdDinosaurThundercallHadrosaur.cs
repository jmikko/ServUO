using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dinosaur, thundercall hadrosaur corpse")]
    public sealed class SrdDinosaurThundercallHadrosaur : SrdMonster
    {
        [Constructable]
        public SrdDinosaurThundercallHadrosaur() : base("DinosaurThundercallHadrosaur") 
        {
        }

        public SrdDinosaurThundercallHadrosaur(Serial serial) : base(serial) { }
    }
}
