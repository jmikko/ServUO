using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aridni corpse")]
    public sealed class SrdAridni : SrdMonster
    {
        [Constructable]
        public SrdAridni() : base("Aridni") 
        {
        }

        public SrdAridni(Serial serial) : base(serial) { }
    }
}
