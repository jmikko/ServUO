using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aphasian abomination corpse")]
    public sealed class SrdAphasianAbomination : SrdMonster
    {
        [Constructable]
        public SrdAphasianAbomination() : base("AphasianAbomination") 
        {
        }

        public SrdAphasianAbomination(Serial serial) : base(serial) { }
    }
}
