using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dread walker excavator corpse")]
    public sealed class SrdDreadWalkerExcavator : SrdMonster
    {
        [Constructable]
        public SrdDreadWalkerExcavator() : base("DreadWalkerExcavator") 
        {
        }

        public SrdDreadWalkerExcavator(Serial serial) : base(serial) { }
    }
}
