using BAMod.Arisu.SkillStates.BaseStates;
using ExtraSkillSlots;
using RoR2BepInExPack.GameAssetPaths;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.Arisu.SkillStates.EXSkills
{
    internal class OverClock : BaseArisuSkillState
    {
        protected override float baseDuration => 0;
        protected override float baseFireDelay => 0;
        protected override float fireTime => 0;

        public override void OnEnter()
        {
            base.OnEnter();
            ArisuMain.exState = ArisuCharacterMain.ArisuEXState.OverClock;
        }
        public override void FixedUpdate()
        {
            var extraInputBankTest = outer.GetComponent<ExtraInputBankTest>();
            base.FixedUpdate();
            if ((extraInputBankTest.extraSkill3.justPressed && fixedAge > 0.5f) || ArisuMain.exState != ArisuCharacterMain.ArisuEXState.OverClock)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            if (ArisuMain.exState == ArisuCharacterMain.ArisuEXState.OverClock)
            {
                ArisuMain.exState = ArisuCharacterMain.ArisuEXState.None;
            }
            base.OnExit();
        }

    }
}
