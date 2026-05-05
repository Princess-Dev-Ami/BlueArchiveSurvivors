using R2API;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.Saori.Content
{
    internal static class SaoriCustomDamageTypes
    {
        public static DamageAPI.ModdedDamageType SaoriExplodeOnHit = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType SaoriBurstBasic = DamageAPI.ReserveDamageType();
    }
}
