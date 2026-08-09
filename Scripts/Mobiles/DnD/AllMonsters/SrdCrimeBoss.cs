using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crime boss corpse")]
    public sealed class SrdCrimeBoss : SrdMonster
    {
        [Constructable]
        public SrdCrimeBoss() : base("CrimeBoss") 
        {
        }

        public SrdCrimeBoss(Serial serial) : base(serial) { }
    }
}
