using BAMod.Arisu.Content;
using BAMod.Arisu.SkillStates.BaseStates;
using EntityStates;
using ExtraSkillSlots;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BAMod.Arisu.SkillStates.EXSkills
{
    internal class CloakEngine : BaseArisuSkillState
    {
        protected override float baseDuration => 0;
        protected override float baseFireDelay => 0;
        protected override float fireTime => 0;
        private float tick;
        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.AddBuff(RoR2Content.Buffs.Cloak);

            characterBody.AddBuff(RoR2Content.Buffs.CloakSpeed);
            ArisuMain.exState = ArisuCharacterMain.ArisuEXState.Cloak;
        }

        public override void FixedUpdate()
        {
            var extraInputBankTest = outer.GetComponent<ExtraInputBankTest>();
            base.FixedUpdate();
            if (!isAuthority) return;
            tick += Time.fixedDeltaTime;
            if (tick >= 0.33f)
            {
                characterBody.AddBuff(ArisuBuffs.ArisuOverheatStack);
                tick -= 0.33f;
            }
            if ((extraInputBankTest.extraSkill1.justPressed && fixedAge > 0.5f) || ArisuMain.exState != ArisuCharacterMain.ArisuEXState.Cloak ||
                EntityStateMachine.FindByCustomName(this.gameObject, "Gun").state.GetType() != typeof(Idle) ||
                EntityStateMachine.FindByCustomName(this.gameObject, "Scope").state.GetType() != typeof(Idle))
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            if (ArisuMain.exState == ArisuCharacterMain.ArisuEXState.Cloak)
            {
                ArisuMain.exState = ArisuCharacterMain.ArisuEXState.None;
            }
            base.OnExit();
            characterBody.RemoveBuff(RoR2Content.Buffs.Cloak);
            characterBody.RemoveBuff(RoR2Content.Buffs.CloakSpeed);
        }
    }
}
