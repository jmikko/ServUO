using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dinosaur, razorfeather raptor corpse")]
    public sealed class SrdDinosaurRazorfeatherRaptor : SrdMonster
    {
        [Constructable]
        public SrdDinosaurRazorfeatherRaptor() : base("DinosaurRazorfeatherRaptor") 
        {
        }

        public SrdDinosaurRazorfeatherRaptor(Serial serial) : base(serial) { }
    }
}
