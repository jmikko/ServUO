using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    // XmlSpawner's bolt-on quest framework (quest tokens, give/kill objectives, the quest gump
    // button). Quests were removed wholesale as legacy UO content with no D&D equivalent, but
    // XmlSpawner's packet overrides and TalkingBaseCreature still route through these three
    // entry points, so they are kept as inert no-ops: nothing is ever registered or awarded.
    public static class XmlQuest
    {
        public static bool RegisterGive(Mobile from, object questgiver, Item item)
        {
            return false;
        }

        public static void RegisterKill(Mobile killed, Mobile killer)
        {
        }

        public static void QuestButton(NetState state, IEntity e, EncodedReader reader)
        {
        }

        public static void QuestButton(QuestGumpRequestArgs e)
        {
        }
    }
}
