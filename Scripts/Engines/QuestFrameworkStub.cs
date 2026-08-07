using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    // The quest system (quest chains, objectives, questers, reward gumps) was legacy UO content
    // with no D&D equivalent and was removed wholesale. These stubs remain only because a handful
    // of surviving files still call into shared quest *utilities* - a gump colour helper, a generic
    // item constructor, and the object serializers the community-collection boxes reuse. No quest
    // can be offered, tracked or completed; every lookup returns null/empty.
    public class BaseQuest
    {
    }

    public class BaseQuestGump : BaseGump
    {
        public BaseQuestGump(PlayerMobile pm, int x = 50, int y = 50)
            : base(pm, x, y)
        {
        }

        public override void AddGumpLayout()
        {
        }
    }

    public static class QuestHelper
    {
        public static BaseQuest GetQuest(PlayerMobile pm, Type questType)
        {
            return null;
        }

        public static object Construct(Type type)
        {
            if (type == null)
            {
                return null;
            }

            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }

        public static bool CheckItem(PlayerMobile pm, Item item)
        {
            return false;
        }
    }

    public static class QuestSystem
    {
        public static void FocusTo(Mobile who, Mobile to)
        {
            who.Direction = who.GetDirectionTo(to);
        }
    }

    // The community-collection donation boxes persist their reward titles through these, so the
    // wire format (a bool discriminator followed by the payload) has to stay stable.
    public static class QuestWriter
    {
        public static void Object(GenericWriter writer, object obj)
        {
            if (obj is string)
            {
                writer.Write(true);
                writer.Write((string)obj);
            }
            else
            {
                writer.Write(false);
                writer.Write(obj is int ? (int)obj : 0);
            }
        }
    }

    public static class QuestReader
    {
        public static object Object(GenericReader reader)
        {
            if (reader.ReadBool())
            {
                return reader.ReadString();
            }

            return reader.ReadInt();
        }
    }
}
