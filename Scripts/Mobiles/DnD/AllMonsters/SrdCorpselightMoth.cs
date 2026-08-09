using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corpselight moth corpse")]
    public sealed class SrdCorpselightMoth : SrdMonster
    {
        [Constructable]
        public SrdCorpselightMoth() : base("CorpselightMoth") 
        {
        }

        public SrdCorpselightMoth(Serial serial) : base(serial) { }
    }
}
