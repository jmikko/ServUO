using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dogmole corpse")]
    public sealed class SrdDogmole : SrdMonster
    {
        [Constructable]
        public SrdDogmole() : base("Dogmole") 
        {
        }

        public SrdDogmole(Serial serial) : base(serial) { }
    }
}
