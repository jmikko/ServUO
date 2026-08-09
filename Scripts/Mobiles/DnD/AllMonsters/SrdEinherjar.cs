using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a einherjar corpse")]
    public sealed class SrdEinherjar : SrdMonster
    {
        [Constructable]
        public SrdEinherjar() : base("Einherjar") 
        {
        }

        public SrdEinherjar(Serial serial) : base(serial) { }
    }
}
