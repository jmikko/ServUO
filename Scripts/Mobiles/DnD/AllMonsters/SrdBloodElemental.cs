using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blood elemental corpse")]
    public sealed class SrdBloodElemental : SrdMonster
    {
        [Constructable]
        public SrdBloodElemental() : base("BloodElemental") 
        {
        }

        public SrdBloodElemental(Serial serial) : base(serial) { }
    }
}
