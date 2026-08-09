using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blaspheming hand corpse")]
    public sealed class SrdBlasphemingHand : SrdMonster
    {
        [Constructable]
        public SrdBlasphemingHand() : base("BlasphemingHand") 
        {
        }

        public SrdBlasphemingHand(Serial serial) : base(serial) { }
    }
}
