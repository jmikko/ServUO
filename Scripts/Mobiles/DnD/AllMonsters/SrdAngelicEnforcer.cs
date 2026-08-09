using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angelic enforcer corpse")]
    public sealed class SrdAngelicEnforcer : SrdMonster
    {
        [Constructable]
        public SrdAngelicEnforcer() : base("AngelicEnforcer") 
        {
        }

        public SrdAngelicEnforcer(Serial serial) : base(serial) { }
    }
}
