using EntityStates.Commando.CommandoWeapon;
using R2API;
using Rewired.Demos;
using RoR2;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using EntityStates;
using BAMod.Arisu.SkillStates.BaseStates;
using BAMod.Arisu.Content;
using Rewired.ComponentControls.Data;
using UnityEngine.Android;

namespace BAMod.Arisu.SkillStates.Primary
{
    internal class RailgunAttackCharge : BaseArisuSkillState
    {
        protected override float baseDuration => 0.1f;
        protected override float baseFireDelay => 0;
        protected override float fireTime => 0;
        private bool fired = false;
        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;
        public GameObject tracerEffectPrefab = FireBarrage.tracerEffectPrefab;
        public DamageType damageType = DamageType.IgniteOnHit;
        private float tick;
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                if (ArisuMain.exState == ArisuCharacterMain.ArisuEXState.OverClock)
                {
                    ArisuMain.beamTime += 0.1f;
                    characterBody.AddBuff(ArisuBuffs.ArisuOverheatStack);
                    ArisuMain.overclockStacks += 1;
                    outer.SetNextStateToMain();
                    return;
                }
                if (fixedAge >= duration)
                {
                    ArisuMain.beamTime += 0.1f;
                    outer.SetNextStateToMain();
                    return;
                }
                else if (!IsKeyDownAuthority())
                {
                    activatorSkillSlot.AddOneStock();
                    outer.SetNextStateToMain();
                    return;
                }
                if (ArisuMain.ultimateGun)
                {
                    skillLocator.primary.AddOneStock();
                    outer.SetNextStateToMain();
                    return;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (skillLocator.primary.stock <= 0)
            {
                ArisuMain.overheat = true;
                ArisuMain.RequestOverride(ArisuCharacterMain.ArisuOverrideRequest.OverheatSwitch);
                skillLocator.primary.cooldownOverride = 0;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
