using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ashen custodian corpse")]
    public sealed class SrdAshenCustodian : SrdMonster
    {
        [Constructable]
        public SrdAshenCustodian() : base("AshenCustodian") 
        {
        }

        public SrdAshenCustodian(Serial serial) : base(serial) { }
    }
}
