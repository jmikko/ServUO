using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alabroza, bloodfiend corpse")]
    public sealed class SrdAlabrozaBloodfiend : SrdMonster
    {
        [Constructable]
        public SrdAlabrozaBloodfiend() : base("AlabrozaBloodfiend") 
        {
        }

        public SrdAlabrozaBloodfiend(Serial serial) : base(serial) { }
    }
}
