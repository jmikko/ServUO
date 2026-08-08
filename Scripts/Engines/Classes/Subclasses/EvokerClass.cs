using System;

namespace Server.Engines.Classes.Subclasses
{
    public class EvokerClass : WizardClass
    {
        public override string Name { get { return "Evoker"; } }
        public override Type ParentClass { get { return typeof(WizardClass); } }
    }
}
