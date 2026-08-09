using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dinosaur, guardian archaeopteryx corpse")]
    public sealed class SrdDinosaurGuardianArchaeopteryx : SrdMonster
    {
        [Constructable]
        public SrdDinosaurGuardianArchaeopteryx() : base("DinosaurGuardianArchaeopteryx") 
        {
        }

        public SrdDinosaurGuardianArchaeopteryx(Serial serial) : base(serial) { }
    }
}
