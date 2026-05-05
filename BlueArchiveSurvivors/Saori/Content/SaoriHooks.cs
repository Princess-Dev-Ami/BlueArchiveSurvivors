using BAMod.GlobalContent.Components;
using BAMod.Mashiro.Content;
using BAMod.Mashiro.SkillStates.BaseStates;
using EntityStates.BrotherMonster;
using Newtonsoft.Json.Utilities;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BAMod.Saori.Content
{
    static class SaoriHooks
    {
        public static void Init()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.HasBuff(SaoriBuffs.PassiveStack))
            {
                var stacks = sender.GetBuffCount(SaoriBuffs.PassiveStack);
                args.attackSpeedMultAdd = 1.5f * stacks;
                args.moveSpeedMultAdd += 0.05f * stacks;
            }
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (!self && damageInfo == null)
            {
                orig(self, damageInfo);
                return;
            }
            if (damageInfo.attacker && damageInfo.attacker.TryGetComponent<CharacterBody>(out var attackerBody))
            {
                if (damageInfo.HasModdedDamageType(SaoriCustomDamageTypes.SaoriBurstBasic) || damageInfo.HasModdedDamageType(SaoriCustomDamageTypes.SaoriExplodeOnHit))
                {
                    if (damageInfo.crit)
                    {
                        attackerBody.AddTimedBuff(SaoriBuffs.PassiveStack, 5f, 10);
                    }
                }
                if (damageInfo.HasModdedDamageType(SaoriCustomDamageTypes.SaoriExplodeOnHit))
                {
                    new BlastAttack()
                    {
                        radius = 5,
                        attacker = damageInfo.attacker,
                        damageColorIndex = damageInfo.damageColorIndex,
                        damageType = DamageType.AOE,
                        baseDamage = damageInfo.damage,
                        crit = damageInfo.crit,
                        inflictor = damageInfo.inflictor,
                        position = self.transform.position,
                        procChainMask = damageInfo.procChainMask,
                        procCoefficient = damageInfo.procCoefficient,
                        teamIndex = attackerBody.teamComponent.teamIndex
                    }.Fire();
                }
            }
            orig(self, damageInfo);
        }
    }
}
