using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chimeric phantom corpse")]
    public sealed class SrdChimericPhantom : SrdMonster
    {
        [Constructable]
        public SrdChimericPhantom() : base("ChimericPhantom") 
        {
        }

        public SrdChimericPhantom(Serial serial) : base(serial) { }
    }
}
