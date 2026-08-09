using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cats of ulthar corpse")]
    public sealed class SrdCatsOfUlthar : SrdMonster
    {
        [Constructable]
        public SrdCatsOfUlthar() : base("CatsOfUlthar") 
        {
        }

        public SrdCatsOfUlthar(Serial serial) : base(serial) { }
    }
}
