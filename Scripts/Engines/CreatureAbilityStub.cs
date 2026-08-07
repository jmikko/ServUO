using System;

namespace Server.Mobiles
{
    // The Pet Training "ability profile" system (magical schools, special attacks and aura effects
    // that tamed creatures could learn) has no D&D equivalent and its implementation was removed
    // along with the rest of the legacy pet content. These types remain because ~44 surviving
    // creature definitions still declare their abilities via SetSpecialAbility(...) etc in their
    // constructors. Every ability here is inert: nothing reads the declarations back, and the
    // trigger hooks are no-ops.
    public class MagicalAbility
    {
        public static readonly MagicalAbility Magery = new MagicalAbility();
        public static readonly MagicalAbility MageryMastery = new MagicalAbility();
        public static readonly MagicalAbility Necromancy = new MagicalAbility();
        public static readonly MagicalAbility Necromage = new MagicalAbility();
        public static readonly MagicalAbility Spellweaving = new MagicalAbility();
        public static readonly MagicalAbility Mysticism = new MagicalAbility();
        public static readonly MagicalAbility Bushido = new MagicalAbility();
        public static readonly MagicalAbility Ninjitsu = new MagicalAbility();
        public static readonly MagicalAbility Chivalry = new MagicalAbility();
        public static readonly MagicalAbility Discordance = new MagicalAbility();
        public static readonly MagicalAbility Poisoning = new MagicalAbility();
    }

    public class SpecialAbility
    {
        public static readonly SpecialAbility Anemia = new SpecialAbility();
        public static readonly SpecialAbility AngryFire = new SpecialAbility();
        public static readonly SpecialAbility ColossalBlow = new SpecialAbility();
        public static readonly SpecialAbility ColossalRage = new SpecialAbility();
        public static readonly SpecialAbility DragonBreath = new SpecialAbility();
        public static readonly SpecialAbility GraspingClaw = new SpecialAbility();
        public static readonly SpecialAbility Heal = new SpecialAbility();
        public static readonly SpecialAbility LifeDrain = new SpecialAbility();
        public static readonly SpecialAbility LifeLeech = new SpecialAbility();
        public static readonly SpecialAbility SearingWounds = new SpecialAbility();
        public static readonly SpecialAbility StickySkin = new SpecialAbility();
        public static readonly SpecialAbility TailSwipe = new SpecialAbility();
        public static readonly SpecialAbility Webbing = new SpecialAbility();

        public void DoEffects(Mobile attacker, Mobile defender, ref int damage)
        {
        }

        public static void CheckCombatTrigger(Mobile attacker, Mobile defender, ref int damage, DamageType type)
        {
        }

        public static void CheckThinkTrigger(BaseCreature bc)
        {
        }

        public static void CheckApproachTrigger(Mobile from, Mobile to, Point3D oldLocation)
        {
        }
    }

    public class AreaEffect
    {
        public static readonly AreaEffect AuraDamage = new AreaEffect();
        public static readonly AreaEffect AuraOfEnergy = new AreaEffect();
        public static readonly AreaEffect PoisonBreath = new AreaEffect();

        public static void CheckThinkTrigger(BaseCreature bc)
        {
        }
    }
}
