using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a demilich mastermind corpse")]
    public sealed class SrdDemilichMastermind : SrdMonster
    {
        [Constructable]
        public SrdDemilichMastermind() : base("DemilichMastermind") 
        {
        }

        public SrdDemilichMastermind(Serial serial) : base(serial) { }
    }
}
