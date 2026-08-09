using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a anophiloi corpse")]
    public sealed class SrdAnophiloi : SrdMonster
    {
        [Constructable]
        public SrdAnophiloi() : base("Anophiloi") 
        {
        }

        public SrdAnophiloi(Serial serial) : base(serial) { }
    }
}
