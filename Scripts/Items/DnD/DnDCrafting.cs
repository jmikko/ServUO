using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.DnDCrafting
{
    public class DnDCraftingGump : Gump
    {
        private Mobile m_From;
        private DnDTool m_Tool;

        public DnDCraftingGump(Mobile from, DnDTool tool) : base(50, 50)
        {
            m_From = from;
            m_Tool = tool;

            AddPage(0);
            AddBackground(0, 0, 400, 300, 5054);
            AddHtml(10, 10, 380, 25, $"<center>D&D Artisan Crafting: {tool}</center>", false, false);
            
            IDnDCharacter character = from as IDnDCharacter;
            if (character == null || !character.DnDInitialized)
            {
                AddHtml(10, 40, 380, 25, "You must be a fully initialized D&D character to craft.", false, false);
                return;
            }

            if (character is DnDPlayerMobile pm && !pm.IsProficient(tool))
            {
                AddHtml(10, 40, 380, 25, $"You are not proficient with {tool}.", false, false);
                return;
            }

            AddHtml(10, 40, 380, 25, "Select an item to produce:", false, false);
            
            int y = 70;
            if (tool == DnDTool.SmithsTools)
            {
                AddButton(10, y, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtml(45, y, 200, 25, "Longsword (Requires 10 Iron Ingots)", false, false);
                y += 30;
            }
            else if (tool == DnDTool.AlchemistsSupplies)
            {
                AddButton(10, y, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtml(45, y, 200, 25, "Potion of Healing (Requires 3 Ginseng)", false, false);
                y += 30;
            }
            else
            {
                AddHtml(10, y, 380, 25, "No recipes available for this tool yet.", false, false);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 0) return;

            IDnDCharacter character = sender.Mobile as IDnDCharacter;
            if (character == null || !character.DnDInitialized) return;

            if (info.ButtonID == 1 && m_Tool == DnDTool.SmithsTools)
            {
                bool success = CombatRules.CheckAbility(sender.Mobile, AbilityScoreType.Str, 12, RollMode.Normal);
                if (success)
                {
                    sender.Mobile.Backpack.DropItem(new Server.Items.DnDLongsword());
                    sender.Mobile.SendMessage("You successfully forge a longsword.");
                }
                else
                {
                    sender.Mobile.SendMessage("You failed to craft the item and ruined the materials.");
                }
            }
            else if (info.ButtonID == 2 && m_Tool == DnDTool.AlchemistsSupplies)
            {
                bool success = CombatRules.CheckAbility(sender.Mobile, AbilityScoreType.Int, 12, RollMode.Normal);
                if (success)
                {
                    sender.Mobile.SendMessage("You successfully brew a healing potion.");
                }
                else
                {
                    sender.Mobile.SendMessage("You ruined the brew.");
                }
            }
            
            sender.Mobile.SendGump(new DnDCraftingGump(sender.Mobile, m_Tool));
        }
    }

    public class DnDArtisanToolItem : Item
    {
        private DnDTool m_Tool;

        [CommandProperty(AccessLevel.GameMaster)]
        public DnDTool Tool
        {
            get { return m_Tool; }
            set { m_Tool = value; InvalidateProperties(); }
        }

        [Constructable]
        public DnDArtisanToolItem() : this(DnDTool.SmithsTools)
        {
        }

        [Constructable]
        public DnDArtisanToolItem(DnDTool tool) : base(0x1EBC) // Tinker tools graphic
        {
            Weight = 5.0;
            m_Tool = tool;
            Name = tool.ToString();
        }

        public DnDArtisanToolItem(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.CloseGump(typeof(DnDCraftingGump));
            from.SendGump(new DnDCraftingGump(from, m_Tool));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((int)m_Tool);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Tool = (DnDTool)reader.ReadInt();
        }
    }
}
