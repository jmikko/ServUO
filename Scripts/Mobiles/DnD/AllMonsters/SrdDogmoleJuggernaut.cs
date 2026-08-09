using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dogmole juggernaut corpse")]
    public sealed class SrdDogmoleJuggernaut : SrdMonster
    {
        [Constructable]
        public SrdDogmoleJuggernaut() : base("DogmoleJuggernaut") 
        {
        }

        public SrdDogmoleJuggernaut(Serial serial) : base(serial) { }
    }
}
