using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cambion corpse")]
    public sealed class SrdCambion : SrdMonster
    {
        [Constructable]
        public SrdCambion() : base("Cambion") 
        {
        }

        public SrdCambion(Serial serial) : base(serial) { }
    }
}
