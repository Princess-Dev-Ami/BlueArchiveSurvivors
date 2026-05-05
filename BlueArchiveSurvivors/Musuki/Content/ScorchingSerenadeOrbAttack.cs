using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.Musuki.Content
{
    internal class ScorchingSerenadeOrbAttack : GenericDamageOrb
    {
        public override void OnArrival()
        {
            base.OnArrival();
            new BlastAttack()
            {
                damageColorIndex = this.damageColorIndex,
                damageType = this.damageType,
                baseDamage = this.damageValue,
                attacker = this.attacker,
                teamIndex = this.teamIndex,
                crit = this.isCrit,
                radius = 10f,
                procChainMask = this.procChainMask,
                procCoefficient = this.procCoefficient,
                falloffModel = BlastAttack.FalloffModel.None,
                position = this.target.transform.position
            }.Fire();
        }
    }
}
