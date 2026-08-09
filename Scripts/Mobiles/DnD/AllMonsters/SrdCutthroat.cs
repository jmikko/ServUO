using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cutthroat corpse")]
    public sealed class SrdCutthroat : SrdMonster
    {
        [Constructable]
        public SrdCutthroat() : base("Cutthroat") 
        {
        }

        public SrdCutthroat(Serial serial) : base(serial) { }
    }
}
