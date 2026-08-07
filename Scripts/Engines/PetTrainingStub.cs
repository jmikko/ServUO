using Server.Mobiles;

namespace Server.Mobiles
{
    // The Pet Training system (taming progression, ability/skill loadouts, control-slot planning)
    // has no D&D equivalent and its implementation was removed with the rest of the legacy pet
    // content. Kept as an always-disabled stub because a scattering of weapon-ability and taming
    // code paths still call into it; every entry point here is inert.
    public class AbilityProfile
    {
        public void OnTame()
        {
        }

        public bool HasAbility(object o)
        {
            return false;
        }
    }

    public class TrainingProfile
    {
        public void CheckProgress(BaseCreature bc)
        {
        }
    }

    public static class PetTrainingHelper
    {
        public static bool Enabled { get { return false; } }

        private static readonly AbilityProfile _Ability = new AbilityProfile();
        private static readonly TrainingProfile _Training = new TrainingProfile();

        public static AbilityProfile GetAbilityProfile(BaseCreature bc, bool create = false)
        {
            return _Ability;
        }

        public static TrainingProfile GetTrainingProfile(BaseCreature bc, bool create = false)
        {
            return _Training;
        }

        public static bool CheckSecondarySkill(BaseCreature bc, SkillName skill)
        {
            return false;
        }

        public static void OnWeaponAbilityUsed(BaseCreature bc, SkillName skill)
        {
        }
    }
}
