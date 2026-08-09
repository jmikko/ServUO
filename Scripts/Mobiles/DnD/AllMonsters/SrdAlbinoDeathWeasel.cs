using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a albino death weasel corpse")]
    public sealed class SrdAlbinoDeathWeasel : SrdMonster
    {
        [Constructable]
        public SrdAlbinoDeathWeasel() : base("AlbinoDeathWeasel") 
        {
        }

        public SrdAlbinoDeathWeasel(Serial serial) : base(serial) { }
    }
}
