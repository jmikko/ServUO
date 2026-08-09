using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chatterlome corpse")]
    public sealed class SrdChatterlome : SrdMonster
    {
        [Constructable]
        public SrdChatterlome() : base("Chatterlome") 
        {
        }

        public SrdChatterlome(Serial serial) : base(serial) { }
    }
}
