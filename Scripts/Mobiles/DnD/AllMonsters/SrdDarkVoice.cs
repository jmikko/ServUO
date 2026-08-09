using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dark voice corpse")]
    public sealed class SrdDarkVoice : SrdMonster
    {
        [Constructable]
        public SrdDarkVoice() : base("DarkVoice") 
        {
        }

        public SrdDarkVoice(Serial serial) : base(serial) { }
    }
}
