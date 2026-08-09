using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ankheg spawn corpse")]
    public sealed class SrdAnkhegSpawn : SrdMonster
    {
        [Constructable]
        public SrdAnkhegSpawn() : base("AnkhegSpawn") 
        {
        }

        public SrdAnkhegSpawn(Serial serial) : base(serial) { }
    }
}
