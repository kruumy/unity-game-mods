using BepInEx;
using BepInEx.Unity.IL2CPP;
using Gatekeeper.CameraScripts.HUD.SkillPanelStuff;
using Gatekeeper.Char_Scripts.General;
using Gatekeeper.Enemy.Base;
using Gatekeeper.NPC;
using HarmonyLib;
using UniRx;
using UnityEngine;

namespace Aimbot
{
    [BepInPlugin("com.kruumy.Aimbot", "Aimbot", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            new Harmony("com.kruumy.Aimbot").PatchAll();
            Log.LogInfo("Aimbot Loaded!");
        }

        public static Vector3? CachedAimbotScreenPoint;
        private static NpcCharacter? FindClosestEnemy( ReactiveCollection<IEnemy> enemies, Vector3 playerPosition )
        {
            float bestDistSq = float.MaxValue;
            NpcCharacter? bestTarget = null;

            for ( int i = 0; i < enemies.Count; i++ )
            {
                IEnemy enemy = enemies[ i ];
                if ( enemy == null )
                    continue;

                NpcCharacter? npc = enemy.TryCast<NpcCharacter>();
                if ( npc == null )
                    continue;

                float distSq = (npc.transform.position - playerPosition).sqrMagnitude;
                if ( distSq < bestDistSq )
                {
                    bestDistSq = distSq;
                    bestTarget = npc;
                }
            }

            return bestTarget;
        }
        private static bool IsOnScreen( Camera cam, Vector3 worldPos, out Vector3 screenPos )
        {
            screenPos = cam.WorldToScreenPoint(worldPos);

            if ( screenPos.z <= 0f )
                return false;

            if ( screenPos.x < 0f || screenPos.x > Screen.width )
                return false;

            if ( screenPos.y < 0f || screenPos.y > Screen.height )
                return false;

            return true;
        }

        [HarmonyPatch(typeof(Gatekeeper.CameraScripts.HUD.Aim.AimController), nameof(Gatekeeper.CameraScripts.HUD.Aim.AimController.GetAimPos))]
        public static class AimController_GetAimPos_Patch
        {
            public const float AimSmoothSpeed = 25f; 

            public static void Postfix( Gatekeeper.CameraScripts.HUD.Aim.AimController __instance, ref Vector3 __result )
            {
                if( __instance.isActiveAndEnabled != false )
                {
                    NpcCharacter? target = FindClosestEnemy( Gatekeeper.General.GameplayManagers.GameplayManager.Instance.EnemySpawner.Enemies, __instance._charManager.transform.position);

                    if ( target != null && IsOnScreen(__instance._mainCamera, target.transform.position, out Vector3 screenPos) )
                    {
                        float t = 1f - Mathf.Exp(-AimSmoothSpeed * Time.deltaTime);
                        Vector3 smoothed = Vector3.Lerp(__result, screenPos, t);

                        __result = smoothed;
                        CachedAimbotScreenPoint = __result;
                        return;
                    }
                }
                CachedAimbotScreenPoint = null;
            }
        }

        [HarmonyPatch(typeof(Gatekeeper.CameraScripts.HUD.Aim.AimController), nameof(Gatekeeper.CameraScripts.HUD.Aim.AimController.LateUpdate))]
        public static class AimController_LateUpdate_Patch
        {
            public static void Postfix( Gatekeeper.CameraScripts.HUD.Aim.AimController __instance )
            {
                if ( CachedAimbotScreenPoint != null )
                {
                    __instance.aimObject.transform.position = (Vector3)CachedAimbotScreenPoint;
                }
            }
        }
    }
}