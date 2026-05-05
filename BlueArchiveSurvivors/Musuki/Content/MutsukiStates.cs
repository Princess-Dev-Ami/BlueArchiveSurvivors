
using BAMod.Mutsuki.SkillStates.Primary;
using BAMod.Mutsuki.SkillStates.Secondary;
using BAMod.Mutsuki.SkillStates.Special;
using BAMod.Mashiro.SkillStates.BaseStates;
namespace BAMod.Mutsuki.Content
{
    public static class MutsukiStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(MutsukiCharacterMain));
            Modules.Content.AddEntityState(typeof(Landmine));
            Modules.Content.AddEntityState(typeof(LMG));
            Modules.Content.AddEntityState(typeof(MutsukiUlt));
        }
    }
}
