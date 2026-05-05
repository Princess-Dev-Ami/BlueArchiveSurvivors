using BAMod.GlobalContent.Components;
using BAMod.Saori.Content;
using BAMod.Saori.SkillStates.BaseStates;
using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI;

namespace BAMod.Saori.SkillStates.Secondary
{
    internal class HERound : BaseSaoriSkillState
    {
        protected override float baseDuration => 1;
        protected override float baseFireDelay => 0.2f;
        protected override float fireTime => 1;
        private bool fired = false;
        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;
        public GameObject tracerEffectPrefab = FireBarrage.tracerEffectPrefab;
        public DamageTypeCombo damageType = DamageType.Generic;
        private int FiredAmount;
        public override void OnEnter()
        {
            base.OnEnter();
            SaoriMain.HERound = true;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                if (!IsKeyDownAuthority())
                {
                    outer.SetNextStateToMain();
                    return;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            SaoriMain.HERound = false;
        }
    }
}
