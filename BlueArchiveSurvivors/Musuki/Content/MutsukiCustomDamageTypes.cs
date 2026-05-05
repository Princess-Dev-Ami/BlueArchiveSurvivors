using R2API;
using RoR2BepInExPack.GameAssetPaths;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.Mutsuki.Content
{
    internal class MutsukiCustomDamageTypes
    {
        public static DamageAPI.ModdedDamageType MutsukiStackCrit = DamageAPI.ReserveDamageType();
    }
}
