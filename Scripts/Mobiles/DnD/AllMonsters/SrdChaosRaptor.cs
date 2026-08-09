using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chaos raptor corpse")]
    public sealed class SrdChaosRaptor : SrdMonster
    {
        [Constructable]
        public SrdChaosRaptor() : base("ChaosRaptor") 
        {
        }

        public SrdChaosRaptor(Serial serial) : base(serial) { }
    }
}
