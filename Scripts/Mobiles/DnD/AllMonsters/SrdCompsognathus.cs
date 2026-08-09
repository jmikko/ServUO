using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a compsognathus corpse")]
    public sealed class SrdCompsognathus : SrdMonster
    {
        [Constructable]
        public SrdCompsognathus() : base("Compsognathus") 
        {
        }

        public SrdCompsognathus(Serial serial) : base(serial) { }
    }
}
