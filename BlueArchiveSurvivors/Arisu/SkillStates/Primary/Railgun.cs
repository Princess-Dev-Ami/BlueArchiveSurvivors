using BAMod.Arisu.Content;
using BAMod.Arisu.SkillStates.BaseStates;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BAMod.Arisu.SkillStates.Primary
{
    internal class Railgun : BaseArisuSkillState
    {
        protected override float baseDuration => 5f;
        protected override float baseFireDelay => 1f;
        protected override float fireTime => 0;
        BullseyeSearch search = new BullseyeSearch();
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!isAuthority) return;
            var rail = new BulletAttack()
            {
                stopperMask = LayerIndex.entityPrecise.mask,
                damage = ArisuMain.ReturnCalcDamage(),
                damageColorIndex = DamageColorIndex.Electrocution,
                maxDistance = float.MaxValue,
                isCrit = RollCrit(),
                falloffModel = BulletAttack.FalloffModel.None,
                hitMask = BulletAttack.defaultHitMask,
                origin = this.transform.position,
                owner = this.gameObject,
                radius = 3,
                aimVector = GetAimRay().direction
            };
            rail.AddModdedDamageType(ArisuCustomDamageTypes.AritsuEnergyChain);
            rail.Fire();
            AddRecoil(0, 20, -40, 0);
            outer.SetNextStateToMain();
            return;
        }
        

        public override void OnExit()
        {
            base.OnExit();
            ArisuMain.railgunForm = false;
            ArisuMain.RequestOverride(ArisuCharacterMain.ArisuOverrideRequest.RailgunSwitch);
            ArisuMain.target = null;
            skillLocator.primary.stock = ArisuMain.Fuel;
            ArisuMain.overHeatTime = 0;
            ArisuMain.beamTime = 0;
        }
        
        private HurtBox SearchForTarget(Ray aimRay)
        {
            search.teamMaskFilter = TeamMask.all;
            search.teamMaskFilter.RemoveTeam(teamComponent.teamIndex);
            search.filterByLoS = true;
            search.searchOrigin = aimRay.origin;
            search.searchDirection = aimRay.direction;
            search.sortMode = BullseyeSearch.SortMode.Angle;
            search.maxDistanceFilter = 200;
            search.maxAngleFilter = 30;
            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);
            return search.GetResults().FirstOrDefault();
        }

    }
}
