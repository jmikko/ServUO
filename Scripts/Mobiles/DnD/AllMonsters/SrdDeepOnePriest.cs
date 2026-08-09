using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep one priest corpse")]
    public sealed class SrdDeepOnePriest : SrdMonster
    {
        [Constructable]
        public SrdDeepOnePriest() : base("DeepOnePriest") 
        {
        }

        public SrdDeepOnePriest(Serial serial) : base(serial) { }
    }
}
