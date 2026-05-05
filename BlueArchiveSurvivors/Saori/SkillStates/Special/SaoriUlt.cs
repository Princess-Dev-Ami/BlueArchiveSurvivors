using BAMod.Saori.SkillStates.BaseStates;
using BAMod.Mashiro.Content;
using BAMod.Mashiro.SkillStates.BaseStates;
using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BAMod.Saori.Content;
using EntityStates.Commando.CommandoWeapon;
using R2API;

namespace BAMod.Saori.SkillStates.Special
{
    internal class SaoriUlt : BaseSaoriSkillState
    {
        protected override float baseDuration => 2;
        protected override float baseFireDelay => 0.11f;
        protected override float fireTime => 0;

        private float tick;
        private int Bullet;
        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;
        public GameObject tracerEffectPrefab = FireBarrage.tracerEffectPrefab;
        public override void OnEnter()
        {
            base.OnEnter();
            skillLocator.primary.SetSkillOverride(this.gameObject, SaoriSurvivor.Lock, GenericSkill.SkillOverridePriority.Default);
            skillLocator.secondary.SetSkillOverride(this.gameObject, SaoriSurvivor.Lock, GenericSkill.SkillOverridePriority.Default);
            skillLocator.utility.SetSkillOverride(this.gameObject, SaoriSurvivor.Lock, GenericSkill.SkillOverridePriority.Default);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            tick += Time.fixedDeltaTime;
            var aimRay = GetAimRay();
            if (isAuthority)
            {
                if (tick > fireDelay && Bullet < 10 * (1 / attackSpeedStat))
                {
                    BulletAttack bullet = new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction * 2,
                        minSpread = 0f,
                        maxSpread = base.characterBody.spreadBloomAngle,
                        bulletCount = 1U,
                        procCoefficient = 1f,
                        damage = base.characterBody.damage * SaoriStaticValues.burstDamage,
                        force = 3,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        tracerEffectPrefab = this.tracerEffectPrefab,
                        hitEffectPrefab = this.hitEffectPrefab,
                        isCrit = true,
                        HitEffectNormal = false,
                        stopperMask = BulletAttack.defaultStopperMask,
                        smartCollision = true,
                        maxDistance = 300f,
                        damageType = DamageType.Generic,
                        radius = 1
                    };
                    bullet.AddModdedDamageType(SaoriCustomDamageTypes.SaoriBurstBasic);
                    bullet.Fire();
                }

                if (fixedAge > duration)
                {

                    outer.SetNextStateToMain();
                    return;
                }
            }
        }
        public override void OnExit()
        {
            skillLocator.primary.UnsetSkillOverride(this.gameObject, SaoriSurvivor.Lock, GenericSkill.SkillOverridePriority.Default);
            skillLocator.secondary.UnsetSkillOverride(this.gameObject, SaoriSurvivor.Lock, GenericSkill.SkillOverridePriority.Default);
            skillLocator.utility.UnsetSkillOverride(this.gameObject, SaoriSurvivor.Lock, GenericSkill.SkillOverridePriority.Default);
            base.OnExit();
        }
    }
}
