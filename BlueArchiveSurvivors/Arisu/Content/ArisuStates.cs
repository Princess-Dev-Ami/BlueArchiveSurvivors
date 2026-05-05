
using BAMod.Arisu.SkillStates.BaseStates;
using BAMod.Arisu.SkillStates.EXSkills;
using BAMod.Arisu.SkillStates.Primary;
using BAMod.Arisu.SkillStates.Secondary;
using BAMod.Arisu.SkillStates.Special;
using BAMod.Mashiro.SkillStates.BaseStates;
namespace BAMod.Arisu.Content
{
    public static class ArisuStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(ArisuCharacterMain));
            Modules.Content.AddEntityState(typeof(CoreEject));
            Modules.Content.AddEntityState(typeof(RailgunAttackCharge));
            Modules.Content.AddEntityState(typeof(RailgunAttackChargeOverheat));
            Modules.Content.AddEntityState(typeof(ArisuUlt));
            Modules.Content.AddEntityState(typeof(ArisuUltBeamAttack));
            Modules.Content.AddEntityState(typeof(ArisuUltBeamAttackOverheat));
            Modules.Content.AddEntityState(typeof(Railgun));
            Modules.Content.AddEntityState(typeof(CloakEngine));
            Modules.Content.AddEntityState(typeof(CoolantTank));
            Modules.Content.AddEntityState(typeof(OverClock));
            Modules.Content.AddEntityState(typeof(ZeroPoint));
        }
    }
}
