using BAMod.Musuki.Content;
using BAMod.Mutsuki.Content;
using BAMod.Mutsuki.SkillStates.BaseStates;
using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using HG;
using R2API;
using Rewired.ComponentControls.Data;
using Rewired.Demos;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BAMod.Mutsuki.SkillStates.Primary
{
    internal class LMG : BaseMutsukiSkillState
    {
        protected override float baseDuration => 4;
        protected override float baseFireDelay => 0.2f;
        protected override float fireTime => 1;
        private bool fired = false;
        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;
        public GameObject tracerEffectPrefab = FireBarrage.tracerEffectPrefab;
        public DamageType damageType = DamageType.IgniteOnHit;
        private float tick;
        private int FiredAmount;
        BullseyeSearch search = new BullseyeSearch();
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                var aimRay = GetAimRay();


                {

                }

                GenericDamageOrb LMGOrb = MutsukiMain.AOELMG ? new ScorchingSerenadeOrbAttack() : new HuntressArrowOrb();
                LMGOrb.damageValue = MutsukiStaticValues.LMGDamage * damageStat;
                LMGOrb.AddModdedDamageType(MutsukiCustomDamageTypes.MutsukiStackCrit);
                LMGOrb.isCrit = RollCrit();
                LMGOrb.attacker = this.gameObject;
                LMGOrb.speed = 120f;
                LMGOrb.teamIndex = this.teamComponent.teamIndex;
                LMGOrb.procCoefficient = 1.0f;
                LMGOrb.procChainMask = new ProcChainMask();

                var target = SearchForTarget(GetAimRay());

                if (MutsukiMain.AOELMG)
                {
                    var vectors = ProjectOnPlane(6, 20, Vector3.up, target.transform.position);
                    LMGOrb.origin = transform.position;
                    LMGOrb.target = target;
                    OrbManager.instance.AddOrb(LMGOrb);

                    foreach (Vector3 vector in vectors)
                    {
                        var SerenadeTarget = GameObject.Instantiate(MutsukiAssets.SerenadeTargetPrefab, position: vector, rotation: new Quaternion(0, 0, 0, 0));
                        LMGOrb.target = SerenadeTarget.GetComponent<HurtBox>();
                        OrbManager.instance.AddOrb(LMGOrb);
                    }
                }
                else
                {
                    LMGOrb.origin = transform.position;
                    LMGOrb.target = target;
                    OrbManager.instance.AddOrb(LMGOrb);
                }
                outer.SetNextStateToMain();
                return;
            }
        }

        private HurtBox SearchForTarget(Ray aimRay)
        {
            search.teamMaskFilter = TeamMask.all;
            search.teamMaskFilter.RemoveTeam(teamComponent.teamIndex);
            search.filterByLoS = true;
            search.searchOrigin = aimRay.origin;
            search.searchDirection = aimRay.direction;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = 200;
            search.maxAngleFilter = 30;
            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);
            return search.GetResults().ToList().GetRandom();
        }
        public List<Vector3> ProjectOnPlane(int amount, int radius, Vector3 PlaneProjection, Vector3 origin)
        {
            var returnList = new List<Vector3>(amount);

            for(int i = 0;  i < amount; i++)
            {
                var point = Random.insideUnitCircle * radius;
                var point3D = new Vector3(point.x, 0, point.y);
                point3D = Vector3.ProjectOnPlane(point3D, PlaneProjection);
                returnList.Add(origin + point3D);
            }
            return returnList;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (skillLocator.primary.stock <= 0)
            {
                MutsukiMain.AOELMG = false;
            }
        }

        public static List<Vector3> ScatterVectors(
            Vector3 forward,
            int pelletCount,
            float maxAngle,
            float randomness = 0.5f
        )
        {
            List<Vector3> directions = new List<Vector3>();

            forward.Normalize();
            Quaternion baseRotation = Quaternion.LookRotation(forward);

            for (int i = 0; i < pelletCount; i++)
            {
                float t = (i + Random.value * randomness) / pelletCount;
                float spreadAngle = maxAngle * Mathf.Sqrt(t);

                float theta = (i * 137.5f) % 360f;
                theta += Random.Range(-180f, 180f) * randomness;

                Quaternion aroundForward = Quaternion.AngleAxis(theta, Vector3.forward);
                Quaternion outwardTilt = Quaternion.AngleAxis(spreadAngle, Vector3.right);

                Vector3 dir = baseRotation * (aroundForward * outwardTilt * Vector3.forward);

                dir = Vector3.Slerp(forward, dir, 0.9f);

                directions.Add(dir.normalized);
            }

            return directions;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
