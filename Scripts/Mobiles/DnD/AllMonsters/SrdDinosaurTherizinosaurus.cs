using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dinosaur, therizinosaurus corpse")]
    public sealed class SrdDinosaurTherizinosaurus : SrdMonster
    {
        [Constructable]
        public SrdDinosaurTherizinosaurus() : base("DinosaurTherizinosaurus") 
        {
        }

        public SrdDinosaurTherizinosaurus(Serial serial) : base(serial) { }
    }
}
