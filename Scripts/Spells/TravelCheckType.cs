namespace Server.Spells
{
    // Region travel-restriction categories. These originally lived alongside the UO Magery
    // recall/gate spells; the spell schools themselves are not being carried over to the D&D
    // ruleset, but BaseRegion still exposes a CheckTravel hook keyed on these categories, and
    // a D&D teleportation effect will want the same hook. Kept as a standalone enum.
    public enum TravelCheckType
    {
        RecallFrom,
        RecallTo,
        GateFrom,
        GateTo,
        Mark,
        TeleportFrom,
        TeleportTo
    }
}
