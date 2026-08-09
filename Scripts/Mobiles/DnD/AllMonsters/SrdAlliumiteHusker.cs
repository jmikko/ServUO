using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alliumite, husker corpse")]
    public sealed class SrdAlliumiteHusker : SrdMonster
    {
        [Constructable]
        public SrdAlliumiteHusker() : base("AlliumiteHusker") 
        {
        }

        public SrdAlliumiteHusker(Serial serial) : base(serial) { }
    }
}
