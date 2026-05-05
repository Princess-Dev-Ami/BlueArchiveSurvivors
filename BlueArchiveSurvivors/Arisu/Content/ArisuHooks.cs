using BAMod.GlobalContent.Components;
using BAMod.Mashiro.Content;
using BAMod.Mashiro.SkillStates.BaseStates;
using EntityStates.BrotherMonster;
using RoR2.Orbs;
using Newtonsoft.Json.Utilities;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using HG;

namespace BAMod.Arisu.Content
{
    static class ArisuHooks
    {
        static BuffDef BleedDebuff;
        public static void Init()
        {
            BleedDebuff = LegacyResourcesAPI.Load<BuffDef>("RoR2/Base/Common/bdBleeding");
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.AddBuff_BuffDef += CharacterBody_AddBuff_BuffDef;
        }

        private static void CharacterBody_AddBuff_BuffDef(On.RoR2.CharacterBody.orig_AddBuff_BuffDef orig, CharacterBody self, BuffDef buffDef)
        {
            if (!self || buffDef == null)
            {
                orig(self, buffDef);
                return;
            }
            if(self.HasBuff(ArisuBuffs.coolantActive) && buffDef == ArisuBuffs.ArisuOverheatStack)
            {
                self.AddTimedBuff(ArisuBuffs.hotCoolant, 20f);
                return;
            }
            orig(self, buffDef);
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if(!self || damageInfo == null)
            {
                orig(self, damageInfo);
                return;
            }
            if (damageInfo.attacker && self.body)
            {
                if(damageInfo.damage > self.combinedHealth * (1f + (self.body.armor / 1000f)))
                {
                    if (damageInfo.HasModdedDamageType(ArisuCustomDamageTypes.AritsuEnergyChain))
                    {
                        if (self.body.isBoss)
                        {
                            damageInfo.damage = damageInfo.damage / 1000f;
                            EffectData effectData = new EffectData
                            {
                                origin = damageInfo.position,
                                rotation = Util.QuaternionSafeLookRotation((damageInfo.force != Vector3.zero) ? damageInfo.force : UnityEngine.Random.onUnitSphere)
                            };
                            EffectManager.SpawnEffect(ArisuAssets.weakEffect, effectData, transmit: true);
                        }
                        var chainSearch = new SphereSearch()
                        {
                            radius = 50,
                            mask = LayerIndex.entityPrecise.mask,
                            origin = self.transform.position,
                        };
                        chainSearch.RefreshCandidates();
                        chainSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(damageInfo.attacker.GetComponent<TeamComponent>().teamIndex));
                        chainSearch.FilterCandidatesByDistinctHurtBoxEntities();
                        var candidates = chainSearch.GetHurtBoxes();
                        var remainingDamage = damageInfo.damage - self.combinedHealth * (1f + (self.body.armor / 1000f));
                        candidates = candidates
                            .Where(hb =>
                                hb != null &&
                                hb.healthComponent != null &&
                                hb.healthComponent.body != null &&
                                hb.healthComponent.body != self.body)
                            .ToArray();

                        if (candidates.Length > 0)
                        {
                            damageInfo.damage -= remainingDamage;
                            orig(self, damageInfo);

                            if (damageInfo.rejected) return;
                            if (self && self.alive) self.Suicide();

                            remainingDamage = remainingDamage / candidates.Length;
                            var orb = new HuntressArrowOrb();
                            orb.attacker = damageInfo.attacker;
                            orb.isCrit = damageInfo.crit;
                            orb.procChainMask = damageInfo.procChainMask;
                            orb.damageValue = remainingDamage;
                            orb.damageColorIndex = DamageColorIndex.Electrocution;
                            orb.procCoefficient = damageInfo.procCoefficient;
                            orb.AddModdedDamageType(ArisuCustomDamageTypes.AritsuEnergyChainContinue);
                            orb.origin = self.transform.position;
                            orb.arrivalTime = 5f;
                            foreach (var candidate in candidates)
                            {
                                orb.target = candidate;
                                OrbManager.instance.AddOrb(orb);
                            }
                        }
                        else
                        {
                            orig(self, damageInfo);
                        }
                        return;
                    }
                    if (damageInfo.HasModdedDamageType(ArisuCustomDamageTypes.AritsuEnergyChainContinue))
                    {
                        if (damageInfo.inflictedHurtbox.healthComponent.body.isBoss)
                        {
                            damageInfo.damage = damageInfo.damage / 1000f;
                            EffectData effectData = new EffectData
                            {
                                origin = damageInfo.position,
                                rotation = Util.QuaternionSafeLookRotation((damageInfo.force != Vector3.zero) ? damageInfo.force : UnityEngine.Random.onUnitSphere)
                            };
                            EffectManager.SpawnEffect(ArisuAssets.weakEffect, effectData, transmit: true);
                        }
                        var chainSearch = new SphereSearch()
                        {
                            radius = 50,
                            mask = LayerIndex.entityPrecise.mask,
                            origin = self.transform.position,
                        };
                        chainSearch.RefreshCandidates();
                        chainSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(damageInfo.attacker.GetComponent<TeamComponent>().teamIndex));
                        var candidates = chainSearch.GetHurtBoxes();
                        var remainingDamage = damageInfo.damage - self.combinedHealth * (1f + (self.body.armor / 100f));
                        candidates = candidates
                            .Where(hb =>
                                hb != null &&
                                hb.healthComponent != null &&
                                hb.healthComponent.body != null &&
                                hb.healthComponent.body != self.body)
                            .ToArray();

                        if (candidates.Length > 0)
                        {
                            damageInfo.damage -= remainingDamage;
                            orig(self, damageInfo);

                            if (damageInfo.rejected) return;
                            if (self && self.alive) self.Suicide();

                            var orb = new HuntressArrowOrb();
                            orb.attacker = damageInfo.attacker;
                            orb.isCrit = damageInfo.crit;
                            orb.procChainMask = damageInfo.procChainMask;
                            orb.damageValue = remainingDamage;
                            orb.damageColorIndex = DamageColorIndex.Electrocution;
                            orb.procCoefficient = damageInfo.procCoefficient;
                            orb.AddModdedDamageType(ArisuCustomDamageTypes.AritsuEnergyChainContinue);
                            orb.origin = self.transform.position;
                            orb.arrivalTime = 5f;
                            orb.target = candidates.GetRandom();
                            OrbManager.instance.AddOrb(orb);
                        }
                        else
                        {
                            orig(self, damageInfo);
                        }
                        return;
                    }

                }
                if (damageInfo.HasModdedDamageType(ArisuCustomDamageTypes.ArisuHyperBeam))
                {
                    var damagePercent = damageInfo.damage;
                    var maxDamage = damageInfo.crit ? damageInfo.attacker.GetComponent<CharacterBody>().damage * 200 : damageInfo.attacker.GetComponent<CharacterBody>().damage * 100;
                    damageInfo.damage = Mathf.Min(damagePercent * self.fullCombinedHealth, maxDamage);
                    damageInfo.damageType = DamageType.BypassArmor;
                    orig(self, damageInfo);
                    return;
                }
            }
            orig(self, damageInfo);
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!sender) return;
            if (sender.HasBuff(ArisuBuffs.Withstand))
            {
                args.armorAdd += 300;
            }
            if (sender.HasBuff(ArisuBuffs.ArisuOverheatStack))
            {
                args.baseCurseAdd += sender.GetBuffCount(ArisuBuffs.ArisuOverheatStack) * 0.01f;
            }
        }
    }
}
