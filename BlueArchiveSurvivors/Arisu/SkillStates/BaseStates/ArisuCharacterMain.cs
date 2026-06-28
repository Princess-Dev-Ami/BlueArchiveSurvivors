using BAMod.Arisu.Content;
using BAMod.Arisu.SkillStates.Primary;
using BAMod.Arisu.SkillStates.Special;
using BAMod.Arisu.SkillStates.Utility;
using BAMod.Mashiro.Content;
using EntityStates;
using ExtraSkillSlots;
using IL.RoR2.Achievements.FalseSon;
using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;
namespace BAMod.Arisu.SkillStates.BaseStates
{
    public class ArisuCharacterMain : GenericCharacterMain
    {
        public enum ArisuEXState
        {
            None = -1,
            Coolant,
            Cloak,
            OverClock,
            ZeroPoint
        }

        public enum ArisuOverrideRequest
        {
            None = -1,
            OverheatSwitch,
            RailgunSwitch,
            StanceSwitch
        }

        private ArisuOverrideRequest overrideRequest = ArisuOverrideRequest.None;
        private bool processedRequest;

        public bool overHeating;
        public float beamTime;
        public float overHeatTime;
        public bool withstand;
        public bool ultimateGun;
        public int Fuel;
        public bool? target;
        public bool overheat;
        public bool railgunForm;
        public float tick;
        public float overclockStacks;
        public ArisuEXState exState;
        private float coolantTick;
        //ArisuCharacterMain.cs code start
        public override void OnEnter()
        {
            base.OnEnter();
            var extraSkillSlots = GetComponent<ExtraSkillLocator>();
            extraSkillSlots.extraFirst.SetSkillOverride(this.gameObject, ArisuSurvivor.Cloak, GenericSkill.SkillOverridePriority.Default);
            extraSkillSlots.extraSecond.SetSkillOverride(this.gameObject, ArisuSurvivor.CoolantTank, GenericSkill.SkillOverridePriority.Default);
            extraSkillSlots.extraThird.SetSkillOverride(this.gameObject, ArisuSurvivor.OverClock, GenericSkill.SkillOverridePriority.Default);
        }

        public override void FixedUpdate()
        {
            if (!isAuthority) return;

            base.FixedUpdate();

            if (inputBank.skill1.justReleased && !ultimateGun && (beamTime > 0 || overHeatTime > 0) && tick <= 0)
            {
                railgunForm = true;
                if (RequestOverride(ArisuOverrideRequest.RailgunSwitch))
                { 
                    tick = 2f;
                }
            }
            else if (!railgunForm)
            { 
                Fuel = skillLocator.primary.stock;
                tick -= Time.fixedDeltaTime;
            }

            if (EntityStateMachine.TryFindByCustomName(this.gameObject, "Gun", out var gun) &&
                EntityStateMachine.TryFindByCustomName(this.gameObject, "Movement", out var utility) &&
                EntityStateMachine.TryFindByCustomName(this.gameObject, "Ult", out var ult))
            {
                if (gun.state.GetType() != typeof(Idle) && gun.state.GetType() != typeof(Railgun))
                {
                    if (!withstand)
                    {
                        characterBody.AddBuff(ArisuBuffs.Withstand);
                        characterBody.AddBuff(RoR2Content.Buffs.Slow50);
                    }
                    withstand = true;
                }

                else if ((withstand && !inputBank.skill1.down) || utility.state.GetType() == typeof(EmergencyCooling))
                {
                    if (characterBody.HasBuff(ArisuBuffs.Withstand))
                    {
                        characterBody.RemoveBuff(ArisuBuffs.Withstand);
                        characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
                    }
                    withstand = false;
                }
            }

            if (overclockStacks >= 5)
            {
                overclockStacks -= 5;
                characterBody.AddBuff(ArisuBuffs.ArisuOverheatStack);
            }

            if(exState == ArisuEXState.Coolant)
            {
                if(characterBody.GetBuffCount(ArisuBuffs.hotCoolant) >= 100)
                {
                    exState = ArisuEXState.None;
                    for(int i = 0; i < 100; i++)
                    {
                        characterBody.RemoveBuff(ArisuBuffs.hotCoolant);
                    }
                    new BlastAttack()
                    {
                        teamIndex = TeamIndex.None,
                        attacker = this.gameObject,
                        damageType = DamageType.AOE,
                        damageColorIndex = DamageColorIndex.Bleed,
                        baseDamage = damageStat * 1000,
                        position = this.transform.position,
                        radius = 50
                    }.Fire();

                    InflictDotInfo dot = new InflictDotInfo()
                    {
                        attackerObject = this.gameObject,
                        damageMultiplier = damageStat * 10,
                        dotIndex = DotController.DotIndex.Burn,
                        duration = 10,
                        totalDamage = damageStat * 10,
                        victimObject = this.gameObject
                    };
                    for (int i = 0;i < 20;i++)
                    {
                        DotController.InflictDot(ref dot);
                    }
                }
            }
            UpdateBeam();

        }

        private void UpdateBeam()
        {
            if (overrideRequest == ArisuOverrideRequest.None) return;
            if (processedRequest) return;
            if (overrideRequest == ArisuOverrideRequest.OverheatSwitch)
            {
                if (ultimateGun)
                {
                    skillLocator.primary.UnsetSkillOverride(this.gameObject, !overheat ? ArisuSurvivor.UltBeamOverheat : ArisuSurvivor.UltBeam, GenericSkill.SkillOverridePriority.Default);
                    skillLocator.primary.SetSkillOverride(this.gameObject, overheat ? ArisuSurvivor.UltBeamOverheat : ArisuSurvivor.UltBeam, GenericSkill.SkillOverridePriority.Default);
                    processedRequest = true;
                }
                else
                {
                    skillLocator.primary.UnsetSkillOverride(this.gameObject, !overheat ? ArisuSurvivor.BeamOverheat : ArisuSurvivor.Beam, GenericSkill.SkillOverridePriority.Default);
                    skillLocator.primary.SetSkillOverride(this.gameObject, overheat ? ArisuSurvivor.BeamOverheat : ArisuSurvivor.Beam, GenericSkill.SkillOverridePriority.Default);

                    processedRequest = true;
                    overrideRequest = ArisuOverrideRequest.None;
                    return;
                }
            }
            if (overrideRequest == ArisuOverrideRequest.RailgunSwitch)
            {
                if (ultimateGun)
                {
                    processedRequest = true;
                    return;
                }

                if (railgunForm)
                {
                    skillLocator.primary.UnsetSkillOverride(this.gameObject, overheat ? ArisuSurvivor.BeamOverheat : ArisuSurvivor.Beam, GenericSkill.SkillOverridePriority.Default);
                    skillLocator.primary.SetSkillOverride(this.gameObject, ArisuSurvivor.Railgun, GenericSkill.SkillOverridePriority.Default);
                    processedRequest = true;
                }
                else
                {
                    skillLocator.primary.UnsetSkillOverride(this.gameObject, ArisuSurvivor.Railgun, GenericSkill.SkillOverridePriority.Default);
                    skillLocator.primary.SetSkillOverride(this.gameObject, ArisuSurvivor.Beam, GenericSkill.SkillOverridePriority.Default);
                    skillLocator.primary.stock = Fuel;
                    
                    processedRequest = true;
                    overrideRequest = ArisuOverrideRequest.None;
                    return;
                }
            }
            if (overrideRequest == ArisuOverrideRequest.StanceSwitch)
            {
                if (overheat)
                {
                    skillLocator.primary.UnsetSkillOverride(this.gameObject, !ultimateGun ? ArisuSurvivor.UltBeamOverheat : ArisuSurvivor.BeamOverheat, GenericSkill.SkillOverridePriority.Default);
                }
                else
                {
                    skillLocator.primary.UnsetSkillOverride(this.gameObject, !ultimateGun ? ArisuSurvivor.UltBeam : ArisuSurvivor.Beam, GenericSkill.SkillOverridePriority.Default);
                }
                skillLocator.primary.SetSkillOverride(this.gameObject, ultimateGun ? ArisuSurvivor.UltBeam : ArisuSurvivor.Beam, GenericSkill.SkillOverridePriority.Default);

                processedRequest = true;
                overrideRequest = ArisuOverrideRequest.None;
                return;
            }
        }

        public bool RequestOverride(ArisuOverrideRequest request)
        {
            if (overrideRequest > request) return false;
            overrideRequest = request;
            processedRequest = false;
            return true;
        }
        
        public float ReturnCalcDamage()
        {
            return ((damageStat * Mathf.Lerp(ArisuStaticValues.baseBeamDamage, ArisuStaticValues.maxBaseBeamDamage, beamTime / 10f)) / attackSpeedStat) + ((ArisuStaticValues.maxBaseBeamDamage * overHeatTime * damageStat) / attackSpeedStat);
        }
        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Vehicle;
        }

    }
}
