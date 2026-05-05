using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BAMod.GlobalContent.Buffs
{
    internal static class GlobalBuffs
    {
        public static BuffDef RejectEverything;
        public static void init()
        {
            RejectEverything = Modules.Content.CreateAndAddBuff("Immortal",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.red,
                false,
                false);
        }
    }
}
