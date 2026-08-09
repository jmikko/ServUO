using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a city watch captain corpse")]
    public sealed class SrdCityWatchCaptain : SrdMonster
    {
        [Constructable]
        public SrdCityWatchCaptain() : base("CityWatchCaptain") 
        {
        }

        public SrdCityWatchCaptain(Serial serial) : base(serial) { }
    }
}
