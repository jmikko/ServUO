using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aboleth, nihilith corpse")]
    public sealed class SrdAbolethNihilith : SrdMonster
    {
        [Constructable]
        public SrdAbolethNihilith() : base("AbolethNihilith") 
        {
        }

        public SrdAbolethNihilith(Serial serial) : base(serial) { }
    }
}
