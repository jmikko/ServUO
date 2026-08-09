using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cadaver sprite corpse")]
    public sealed class SrdCadaverSprite : SrdMonster
    {
        [Constructable]
        public SrdCadaverSprite() : base("CadaverSprite") 
        {
        }

        public SrdCadaverSprite(Serial serial) : base(serial) { }
    }
}
