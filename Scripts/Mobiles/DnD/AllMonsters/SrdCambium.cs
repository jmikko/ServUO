using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cambium corpse")]
    public sealed class SrdCambium : SrdMonster
    {
        [Constructable]
        public SrdCambium() : base("Cambium") 
        {
        }

        public SrdCambium(Serial serial) : base(serial) { }
    }
}
