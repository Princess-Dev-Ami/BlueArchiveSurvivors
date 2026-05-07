using BAMod.GlobalContent.Components;
using BAMod.Mashiro.Content;
using BAMod.Modules;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

namespace BAMod.Mutsuki.Content
{
    public static class MutsukiAssets
    {
        
        public static GameObject SerenadeTargetPrefab;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject flameGrenadePrefab;

        public static Sprite SchoolgirlSoulConsume;

        private static AssetBundle _assetBundle;
        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            CreateEffects();

            CreateProjectiles();

            GrabBuffIcons();
        }

        #region effects
        private static void CreateEffects()
        {

        }
        #endregion effects

        #region buffs
        private static void GrabBuffIcons()
        {

        }

        #endregion buffs

        #region projectiles
        private static void CreateProjectiles()
        {
            SerenadeTargetPrefab = PrefabAPI.InstantiateClone(_assetBundle.LoadAsset<GameObject>("SerenadeBlank"), "Serenade Target");
            SerenadeTargetPrefab.AddComponent<CharacterBody>();
            SerenadeTargetPrefab.AddComponent<HurtBox>();
            SerenadeTargetPrefab.AddComponent<Collider>().isTrigger = true;
            SerenadeTargetPrefab.AddComponent<HealthComponent>();
            SerenadeTargetPrefab.AddComponent<NetworkIdentity>();
            SerenadeTargetPrefab.AddComponent<EffectiveImmortal>();
        }

        #endregion projectiles
    }
}
