using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ahuizotl corpse")]
    public sealed class SrdAhuizotl : SrdMonster
    {
        [Constructable]
        public SrdAhuizotl() : base("Ahuizotl") 
        {
        }

        public SrdAhuizotl(Serial serial) : base(serial) { }
    }
}
