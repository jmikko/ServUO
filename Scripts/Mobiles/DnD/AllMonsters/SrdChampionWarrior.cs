using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a champion warrior corpse")]
    public sealed class SrdChampionWarrior : SrdMonster
    {
        [Constructable]
        public SrdChampionWarrior() : base("ChampionWarrior") 
        {
        }

        public SrdChampionWarrior(Serial serial) : base(serial) { }
    }
}
