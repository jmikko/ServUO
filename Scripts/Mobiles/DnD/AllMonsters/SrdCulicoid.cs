using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a culicoid corpse")]
    public sealed class SrdCulicoid : SrdMonster
    {
        [Constructable]
        public SrdCulicoid() : base("Culicoid") 
        {
        }

        public SrdCulicoid(Serial serial) : base(serial) { }
    }
}
