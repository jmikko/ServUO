using Server.Mobiles;

namespace Server.SkillHandlers
{
    // Declared by the Remove Trap skill handler, which was removed (Remove Trap is not a D&D
    // skill). A couple of surviving trap items still advertise the interface, so it is kept
    // here as an empty marker.
    public interface IRemoveTrapTrainingKit
    {
        void OnRemoveTrap(Mobile m);
    }
}

namespace Server.Items
{
    // Declared by the Detect Hidden skill handler, which was removed (Detect Hidden is not a
    // D&D skill; Perception replaces it). Faction/VvV traps and a few hidden items still
    // implement it so that a future D&D Perception check has something to key off.
    public interface IRevealableItem
    {
        bool CheckReveal(Mobile m);
        bool CheckPassiveDetect(Mobile m);
        void OnRevealed(Mobile m);
    }
}
