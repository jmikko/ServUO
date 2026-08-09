using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a child of the briar corpse")]
    public sealed class SrdChildOfTheBriar : SrdMonster
    {
        [Constructable]
        public SrdChildOfTheBriar() : base("ChildOfTheBriar") 
        {
        }

        public SrdChildOfTheBriar(Serial serial) : base(serial) { }
    }
}
