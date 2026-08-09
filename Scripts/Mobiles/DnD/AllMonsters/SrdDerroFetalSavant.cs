using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derro fetal savant corpse")]
    public sealed class SrdDerroFetalSavant : SrdMonster
    {
        [Constructable]
        public SrdDerroFetalSavant() : base("DerroFetalSavant") 
        {
        }

        public SrdDerroFetalSavant(Serial serial) : base(serial) { }
    }
}
