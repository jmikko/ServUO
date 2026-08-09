using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a demon, kogukhpak corpse")]
    public sealed class SrdDemonKogukhpak : SrdMonster
    {
        [Constructable]
        public SrdDemonKogukhpak() : base("DemonKogukhpak") 
        {
        }

        public SrdDemonKogukhpak(Serial serial) : base(serial) { }
    }
}
