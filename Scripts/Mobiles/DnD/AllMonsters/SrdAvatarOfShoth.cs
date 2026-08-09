using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a avatar of shoth corpse")]
    public sealed class SrdAvatarOfShoth : SrdMonster
    {
        [Constructable]
        public SrdAvatarOfShoth() : base("AvatarOfShoth") 
        {
        }

        public SrdAvatarOfShoth(Serial serial) : base(serial) { }
    }
}
