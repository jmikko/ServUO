using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angel, zirnitran corpse")]
    public sealed class SrdAngelZirnitran : SrdMonster
    {
        [Constructable]
        public SrdAngelZirnitran() : base("AngelZirnitran") 
        {
        }

        public SrdAngelZirnitran(Serial serial) : base(serial) { }
    }
}
