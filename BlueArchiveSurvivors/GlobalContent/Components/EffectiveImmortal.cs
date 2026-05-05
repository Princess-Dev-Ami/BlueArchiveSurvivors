using BAMod.GlobalContent.Buffs;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BAMod.GlobalContent.Components
{
    internal class EffectiveImmortal : MonoBehaviour
    {
        public void OnEnable()
        {
            if (TryGetComponent<CharacterBody>(out var toImmortalize))
            {
                toImmortalize.AddBuff(GlobalBuffs.RejectEverything);
            }
        }
    }
}
