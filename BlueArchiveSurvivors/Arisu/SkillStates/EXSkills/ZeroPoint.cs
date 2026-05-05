using BAMod.Arisu.SkillStates.BaseStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.Arisu.SkillStates.EXSkills
{
    internal class ZeroPoint : BaseArisuSkillState
    {
        protected override float baseDuration => 0;
        protected override float baseFireDelay => 0;
        protected override float fireTime => 0;

        public override void OnEnter()
        {
            base.OnEnter();
            ArisuMain.exState = ArisuCharacterMain.ArisuEXState.ZeroPoint;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsKeyJustPressedAuthority() || ArisuMain.exState != ArisuCharacterMain.ArisuEXState.ZeroPoint)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            if (ArisuMain.exState == ArisuCharacterMain.ArisuEXState.ZeroPoint)
            {
                ArisuMain.exState = ArisuCharacterMain.ArisuEXState.None;
            }
            base.OnExit();
        }
    }
}
