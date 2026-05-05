using BAMod.Arisu.Components;
using BAMod.GlobalContent.Components;
using BAMod.Mashiro.Content;
using BAMod.Modules;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2BepInExPack.GameAssetPaths;
using TMPro;
using UnityEngine;

namespace BAMod.Arisu.Content
{
    public static class ArisuAssets
    {
        
        public static GameObject coreExplosionPrefab;
        public static GameObject hud;
        public static GameObject weakEffect;
        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

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
            hud = PrefabAPI.CreateEmptyPrefab("ArisuHud");
            var rectTransform = hud.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;

            weakEffect = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/BearProc"), "Weak");

            if (weakEffect != null)
            {
                TextMeshPro tmp = weakEffect.GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = "Weak!";
                    tmp.fontSize = 12f;
                    tmp.color = Color.red;
                    tmp.alignment = TextAlignmentOptions.Center;

                }
            }

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
            coreExplosionPrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "ArisuCoreEjection");

            UnityEngine.Object.Destroy(coreExplosionPrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = coreExplosionPrefab.AddComponent<ProjectileImpactExplosion>();

            bombImpactExplosion.blastRadius = 30f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.5f;

            ProjectileController bombController = coreExplosionPrefab.GetComponent<ProjectileController>();
            bombController.ghostPrefab = _assetBundle.LoadAsset<GameObject>("AritsuCoreEjectionGhost");
        }

        #endregion projectiles
    }
}
