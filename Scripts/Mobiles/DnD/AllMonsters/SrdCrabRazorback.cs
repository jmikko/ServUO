using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crab, razorback corpse")]
    public sealed class SrdCrabRazorback : SrdMonster
    {
        [Constructable]
        public SrdCrabRazorback() : base("CrabRazorback") 
        {
        }

        public SrdCrabRazorback(Serial serial) : base(serial) { }
    }
}
