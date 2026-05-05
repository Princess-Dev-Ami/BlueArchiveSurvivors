using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.GlobalContent.Buffs
{
    internal static class GlobalBuffHooks
    {
        public static void Init()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            if (self && damageInfo != null && self.body && self.body.HasBuff(GlobalBuffs.RejectEverything))
            {
                damageInfo.rejected = true;
            }
            orig(self, damageInfo);
        }
    }
}
