using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blood imp corpse")]
    public sealed class SrdBloodImp : SrdMonster
    {
        [Constructable]
        public SrdBloodImp() : base("BloodImp") 
        {
        }

        public SrdBloodImp(Serial serial) : base(serial) { }
    }
}
