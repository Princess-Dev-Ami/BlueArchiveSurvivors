using R2API;
using RoR2;
using System.Data.SqlTypes;
using UnityEngine;

namespace BAMod.Saori.Content
{
    public static class SaoriBuffs
    {
        public static BuffDef PassiveStack;
        public static void Init()
        {
            PassiveStack = Modules.Content.CreateAndAddBuff(
                "SaoriPassiveStack",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/BanditSkull").iconSprite,
                Color.white,
                true,
                false);
        }
    }
}

