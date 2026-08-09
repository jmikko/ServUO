using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a avalanche screamer corpse")]
    public sealed class SrdAvalancheScreamer : SrdMonster
    {
        [Constructable]
        public SrdAvalancheScreamer() : base("AvalancheScreamer") 
        {
        }

        public SrdAvalancheScreamer(Serial serial) : base(serial) { }
    }
}
