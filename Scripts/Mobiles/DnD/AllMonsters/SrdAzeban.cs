using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a azeban corpse")]
    public sealed class SrdAzeban : SrdMonster
    {
        [Constructable]
        public SrdAzeban() : base("Azeban") 
        {
        }

        public SrdAzeban(Serial serial) : base(serial) { }
    }
}
