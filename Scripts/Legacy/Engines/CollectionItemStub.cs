using System;
using Server.Mobiles;

namespace Server
{
    // Shared "reward catalog entry" record used by BaseRewardGump and the points-reward systems
    // that survive (BulkOrders, CleanUpBritannia, Blackthorn, VoidPool, ViceVsVirtue). Its original
    // home - Scripts/Services/CommunityCollections - was the museum/zoo/library donation reward
    // economy and was removed wholesale as legacy UO flavor with no D&D equivalent, so the record
    // type is reconstructed here; shape inferred from every surviving call site.
    public class CollectionItem
    {
        public Type Type { get; private set; }
        public int ItemID { get; private set; }
        public int Tooltip { get; private set; }
        public int Hue { get; private set; }
        public double Points { get; private set; }
        public bool QuestItem { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public CollectionItem(Type type, int itemID, int tooltip, int hue, double points, bool questItem = false)
        {
            Type = type;
            ItemID = itemID;
            Tooltip = tooltip;
            Hue = hue;
            Points = points;
            QuestItem = questItem;

            Rectangle2D bounds = ItemBounds.Table[itemID & 0x3FFF];

            X = bounds.X;
            Y = bounds.Y;
            Width = bounds.Width;
            Height = bounds.Height;
        }

        public virtual bool CanSelect(PlayerMobile from)
        {
            return true;
        }

        public virtual void OnGiveReward(PlayerMobile to, Item item, object collection, int hue)
        {
        }
    }
}
