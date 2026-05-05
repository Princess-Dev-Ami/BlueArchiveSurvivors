using BAMod.Arisu.Content;
using BAMod.Arisu.SkillStates.BaseStates;
using BAMod.Mashiro.Content;
using EntityStates;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BAMod.Arisu.SkillStates.Special
{
    internal class ArisuUltBeamAttack : BaseArisuSkillState
    {
        protected override float baseDuration => 0.1f;
        protected override float baseFireDelay => 0;
        protected override float fireTime => 0;

        private bool fired = false;
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
                if (!fired)
                {
                    var aimRay = GetAimRay();
                    var beamAttack = new BulletAttack()
                    {
                        damage = ArisuStaticValues.hyperBeamDamage,
                        damageType = DamageType.Generic,
                        damageColorIndex = DamageColorIndex.Electrocution,
                        maxDistance = 300f,
                        _maxDistance = 300f,
                        falloffModel = BulletAttack.FalloffModel.None,
                        hitMask = BulletAttack.defaultHitMask,
                        tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("RoR2/DLC1/Railgunner/TracerRailgunCryo"),
                        radius = 2,
                        origin = aimRay.origin + aimRay.direction * 2,
                        aimVector = aimRay.direction,
                        stopperMask = LayerIndex.world.mask,
                        isCrit = RollCrit(),
                        procCoefficient = 1.0f,
                        owner = this.gameObject
                    };
                    beamAttack.AddModdedDamageType(ArisuCustomDamageTypes.ArisuHyperBeam);
                    beamAttack.Fire();
                    fired = true;
                    characterBody.AddBuff(ArisuBuffs.ArisuOverheatStack);
                }

                if (fixedAge > duration)
                {
                    if (skillLocator.primary.stock <= 0)
                    {
                        ArisuMain.overheat = true;
                        ArisuMain.RequestOverride(ArisuCharacterMain.ArisuOverrideRequest.OverheatSwitch);
                    }
                    outer.SetNextStateToMain();
                    return;
                }
                else if (!IsKeyDownAuthority())
                {
                    activatorSkillSlot.AddOneStock();
                    outer.SetNextStateToMain();
                    return;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
