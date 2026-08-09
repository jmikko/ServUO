using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dead mans fingers corpse")]
    public sealed class SrdDeadMansFingers : SrdMonster
    {
        [Constructable]
        public SrdDeadMansFingers() : base("DeadMansFingers") 
        {
        }

        public SrdDeadMansFingers(Serial serial) : base(serial) { }
    }
}
