using BAMod.GlobalContent.Components;
using BAMod.Mashiro.Content;
using BAMod.Mashiro.SkillStates.BaseStates;
using BAMod.Saori.Content;
using EntityStates.BrotherMonster;
using Newtonsoft.Json.Utilities;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BAMod.Mutsuki.Content
{
    static class MutsukiHooks
    {
        static BuffDef BleedDebuff;

        public static void Init()
        {
            BleedDebuff = LegacyResourcesAPI.Load<BuffDef>("RoR2/Base/Common/bdBleeding");
        }
    }
}
