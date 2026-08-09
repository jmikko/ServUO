using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.DnDCrafting;

namespace Server.Items
{
    public class DnDThievesToolsItem : Item
    {
        [Constructable]
        public DnDThievesToolsItem() : base(0x1EBC) // Tinker tools graphic
        {
            Weight = 1.0;
            Name = "Thieves' Tools";
        }

        public DnDThievesToolsItem(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            IDnDCharacter character = from as IDnDCharacter;
            if (character == null) return;

            bool isProficient = (character as Server.Mobiles.DnDPlayerMobile)?.IsProficient(DnDTool.ThievesTools) ?? false;
            
            from.SendMessage("Target a lock to pick.");
            from.Target = new Server.Targets.PickTarget(this, isProficient);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DnDHealersKitItem : Item
    {
        private int m_Uses = 10;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Uses { get { return m_Uses; } set { m_Uses = value; InvalidateProperties(); } }

        [Constructable]
        public DnDHealersKitItem() : base(0xE21) // Bandage graphic
        {
            Weight = 3.0;
            Name = "Healer's Kit";
            Hue = 0x20;
        }

        public DnDHealersKitItem(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (m_Uses <= 0)
            {
                from.SendMessage("This kit is empty.");
                return;
            }

            IDnDCharacter character = from as IDnDCharacter;
            if (character == null) return;

            bool isProficient = (character as Server.Mobiles.DnDPlayerMobile)?.IsProficient(DnDSkill.Medicine) ?? false;

            from.SendMessage("Target a wounded creature to stabilize or heal.");
            from.Target = new Server.Targets.HealTarget(this, isProficient);
        }

        public void ConsumeUse(Mobile from)
        {
            m_Uses--;
            if (m_Uses <= 0)
            {
                from.SendMessage("You have exhausted the healer's kit.");
                Delete();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060584, m_Uses.ToString()); // uses remaining: ~1_val~
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Uses);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Uses = reader.ReadInt();
        }
    }
}

namespace Server.Targets
{
    public class PickTarget : Server.Targeting.Target
    {
        private DnDThievesToolsItem m_Item;
        private bool m_IsProficient;

        public PickTarget(DnDThievesToolsItem item, bool isProficient) : base(1, false, Server.Targeting.TargetFlags.None)
        {
            m_Item = item;
            m_IsProficient = isProficient;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Item.Deleted) return;

            System.Reflection.PropertyInfo lockedProp = targeted.GetType().GetProperty("Locked");
            if (lockedProp != null)
            {
                bool isLocked = (bool)lockedProp.GetValue(targeted);
                if (!isLocked)
                {
                    from.SendMessage("That is already unlocked.");
                    return;
                }

                int dc = 15; // Standard lock difficulty since we can't reliably read RequiredSkill
                int roll = Utility.RandomMinMax(1, 20);
                
                int modifier = 0;
                IDnDCharacter character = from as IDnDCharacter;
                if (character != null)
                {
                    modifier += (character.AbilityScores.Dex - 10) / 2;
                    if (m_IsProficient)
                    {
                        modifier += character.PrimaryClass.GetProficiencyBonus(character.TotalLevel);
                    }
                }

                int total = roll + modifier;
                from.SendMessage($"You rolled a {roll} + {modifier} = {total} (DC {dc})");

                if (total >= dc)
                {
                    lockedProp.SetValue(targeted, false);
                    from.SendMessage("You successfully pick the lock.");
                    Effects.PlaySound(from.Location, from.Map, 0x241);
                }
                else
                {
                    from.SendMessage("You failed to pick the lock.");
                }
            }
            else
            {
                from.SendMessage("You cannot pick that.");
            }
        }
    }

    public class HealTarget : Server.Targeting.Target
    {
        private DnDHealersKitItem m_Item;
        private bool m_IsProficient;

        public HealTarget(DnDHealersKitItem item, bool isProficient) : base(1, false, Server.Targeting.TargetFlags.Beneficial)
        {
            m_Item = item;
            m_IsProficient = isProficient;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Item.Deleted) return;

            Mobile target = targeted as Mobile;
            if (target != null)
            {
                if (target.Hits >= target.HitsMax)
                {
                    from.SendMessage("They are not wounded.");
                    return;
                }

                int roll = Utility.RandomMinMax(1, 20);
                int modifier = 0;
                IDnDCharacter character = from as IDnDCharacter;
                if (character != null)
                {
                    modifier += (character.AbilityScores.Wis - 10) / 2;
                    if (m_IsProficient) modifier += character.PrimaryClass.GetProficiencyBonus(character.TotalLevel);
                }

                int total = roll + modifier;
                int dc = 10;
                from.SendMessage($"You rolled a {roll} + {modifier} = {total} (DC {dc})");

                if (total >= dc)
                {
                    int heal = Utility.RandomMinMax(1, 4) + modifier;
                    if (heal <= 0) heal = 1;

                    target.Heal(heal);
                    from.SendMessage($"You successfully bandage their wounds, restoring {heal} hit points.");
                    target.SendMessage($"{from.Name} tends to your wounds.");
                }
                else
                {
                    from.SendMessage("You fail to properly apply the bandages.");
                }

                m_Item.ConsumeUse(from);
            }
            else
            {
                from.SendMessage("You can only use this on living creatures.");
            }
        }
    }
}
