using BAMod.Arisu.Content;
using BAMod.Arisu.SkillStates.BaseStates;
using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BAMod.Arisu.SkillStates.Primary
{
    internal class RailgunAttackChargeOverheat : BaseArisuSkillState
    {
        protected override float baseDuration => 0f;
        protected override float baseFireDelay => 0;
        protected override float fireTime => 0;
        private float tick;
        private float damageTick;
        public override void OnEnter()
        {
            base.OnEnter();
            characterMotor.enabled = false;
            ArisuMain.overheat = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                ArisuMain.overHeatTime += 0.1f;
                damageTick += Time.fixedDeltaTime;
                if (damageTick > 0.2f)
                {
                    characterBody.AddBuff(ArisuBuffs.ArisuOverheatStack);
                    damageTick -= 0.2f;
                }
                else if (!IsKeyDownAuthority())
                {
                    ArisuMain.overheat = false;
                    ArisuMain.RequestOverride(ArisuCharacterMain.ArisuOverrideRequest.RailgunSwitch);
                    outer.SetNextStateToMain();
                    return;
                }
            }
 
        }

        public override void OnExit()
        {
            base.OnExit();
            characterMotor.enabled = true;
            characterMotor.velocity = Vector3.zero;
            ArisuMain.overheat = false;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
