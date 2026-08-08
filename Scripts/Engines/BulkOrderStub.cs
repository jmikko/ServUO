using System;
using Server.Mobiles;

namespace Server.Engines.BulkOrders
{
    // Bulk Order Deeds - the UO crafting-quest economy where crafter NPCs hand out "make N of
    // item X" contracts redeemed for reward currency. No D&D equivalent; the implementation was
    // removed. BaseVendor still declares a BOD category and a bribe/redeem code path, so the
    // shared shapes are kept here as inert stubs: no deed type exists, so nothing can be
    // offered, bribed or redeemed.
    public enum BODType
    {
        Smith,
        Tailor,
        Alchemy,
        Inscription,
        Tinkering,
        Fletching,
        Carpentry,
        Cooking
    }

    public interface IBOD
    {
    }

    public class PendingBribe
    {
        public IBOD BOD { get; set; }
        public int Amount { get; set; }
    }

    public static class BulkOrderSystem
    {
        public static void MutateBOD(IBOD bod) { }
    }
}
