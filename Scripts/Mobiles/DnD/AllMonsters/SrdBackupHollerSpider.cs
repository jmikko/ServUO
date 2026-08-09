using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a backup holler spider corpse")]
    public sealed class SrdBackupHollerSpider : SrdMonster
    {
        [Constructable]
        public SrdBackupHollerSpider() : base("BackupHollerSpider") 
        {
        }

        public SrdBackupHollerSpider(Serial serial) : base(serial) { }
    }
}
