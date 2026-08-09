using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a djinni corpse")]
    public sealed class SrdDjinni : SrdMonster
    {
        [Constructable]
        public SrdDjinni() : base("Djinni") 
        {
        }

        public SrdDjinni(Serial serial) : base(serial) { }
    }
}
