using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alliumite, rapscallion corpse")]
    public sealed class SrdAlliumiteRapscallion : SrdMonster
    {
        [Constructable]
        public SrdAlliumiteRapscallion() : base("AlliumiteRapscallion") 
        {
        }

        public SrdAlliumiteRapscallion(Serial serial) : base(serial) { }
    }
}
