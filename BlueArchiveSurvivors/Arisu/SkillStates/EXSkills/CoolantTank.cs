using BAMod.Arisu.Content;
using BAMod.Arisu.SkillStates.BaseStates;
using ExtraSkillSlots;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.Arisu.SkillStates.EXSkills
{
    internal class CoolantTank : BaseArisuSkillState
    {
        protected override float baseDuration => 0;
        protected override float baseFireDelay => 0;
        protected override float fireTime => 0;

        public override void OnEnter()
        {
            base.OnEnter();
            ArisuMain.exState = ArisuCharacterMain.ArisuEXState.Coolant;
            characterBody.AddBuff(ArisuBuffs.coolantActive);
        }
        public override void FixedUpdate()
        {
            var extraInputBankTest = outer.GetComponent<ExtraInputBankTest>();
            base.FixedUpdate();
            if (!isAuthority) return;
            if ((extraInputBankTest.extraSkill2.justPressed && fixedAge > 0.5f) || ArisuMain.exState != ArisuCharacterMain.ArisuEXState.Coolant)
            {
                outer.SetNextStateToMain();
                return;
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            if (ArisuMain.exState == ArisuCharacterMain.ArisuEXState.Coolant)
            {
                ArisuMain.exState = ArisuCharacterMain.ArisuEXState.None;
            }
            characterBody.RemoveBuff(ArisuBuffs.coolantActive);
        }
    }
}
