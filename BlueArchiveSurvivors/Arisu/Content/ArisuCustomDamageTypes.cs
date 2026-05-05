using R2API;
using RoR2BepInExPack.GameAssetPaths;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.Arisu.Content
{
    internal static class ArisuCustomDamageTypes
    {
        public static DamageAPI.ModdedDamageType AritsuEnergyChain = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType AritsuEnergyChainContinue = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType ArisuHyperBeam = DamageAPI.ReserveDamageType();
    }
}
