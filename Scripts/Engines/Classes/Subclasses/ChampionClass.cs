using System;

namespace Server.Engines.Classes.Subclasses
{
    public class ChampionClass : FighterClass
    {
        public override string Name { get { return "Champion"; } }
        public override Type ParentClass { get { return typeof(FighterClass); } }
    }
}
