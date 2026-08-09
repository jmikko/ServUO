using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arachnocrat corpse")]
    public sealed class SrdArachnocrat : SrdMonster
    {
        [Constructable]
        public SrdArachnocrat() : base("Arachnocrat") 
        {
        }

        public SrdArachnocrat(Serial serial) : base(serial) { }
    }
}
