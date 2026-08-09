using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aboleth thrall corpse")]
    public sealed class SrdAbolethThrall : SrdMonster
    {
        [Constructable]
        public SrdAbolethThrall() : base("AbolethThrall") 
        {
        }

        public SrdAbolethThrall(Serial serial) : base(serial) { }
    }
}
