using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a caretaker weevil corpse")]
    public sealed class SrdCaretakerWeevil : SrdMonster
    {
        [Constructable]
        public SrdCaretakerWeevil() : base("CaretakerWeevil") 
        {
        }

        public SrdCaretakerWeevil(Serial serial) : base(serial) { }
    }
}
