using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a child of yggdrasil corpse")]
    public sealed class SrdChildOfYggdrasil : SrdMonster
    {
        [Constructable]
        public SrdChildOfYggdrasil() : base("ChildOfYggdrasil") 
        {
        }

        public SrdChildOfYggdrasil(Serial serial) : base(serial) { }
    }
}
