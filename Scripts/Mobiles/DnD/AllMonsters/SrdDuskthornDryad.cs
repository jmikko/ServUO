using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a duskthorn dryad corpse")]
    public sealed class SrdDuskthornDryad : SrdMonster
    {
        [Constructable]
        public SrdDuskthornDryad() : base("DuskthornDryad") 
        {
        }

        public SrdDuskthornDryad(Serial serial) : base(serial) { }
    }
}
