using HarmonyLib;
using ModdingUtils.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UnboundLib;
using UnityEngine;

namespace Ported_FFC.Monobehaviors // Piercing bullet logic from TRT
{
    [HarmonyPatch(typeof(ProjectileCollision), "Die")]
    class ProjectileCollision_Patch_Die
    {
        static void Postfix(ProjectileCollision __instance)
        {
            if (__instance.GetComponentInParent<ProjectileHit>()?.ownWeapon?.GetComponent<Gun>()?.GetData().pierce ?? false)
            {
                __instance.SetFieldValue("hasCollided", false);
            }
        }
    }

    [HarmonyPatch(typeof(Gun), "ResetStats")]
    class GunPatchResetStats
    {
        private static void Prefix(Gun __instance)
        {
            __instance.GetData().pierce = false;
        }
    }

    public class GunAdditionalData
    {
        public bool pierce = false;
    }

    public static class GunExtensions
    {
        private static readonly ConditionalWeakTable<Gun, GunAdditionalData> additionalData = new ConditionalWeakTable<Gun, GunAdditionalData>();
        public static GunAdditionalData GetData(this Gun instance)
        {
            return additionalData.GetOrCreateValue(instance);
        }
    }

    [HarmonyPatch(typeof(ProjectileHit), nameof(ProjectileHit.RPCA_DoHit))]
    class ProjectileHit_Patch_RPCA_DoHit_Pierce
    {
        // patch to only call DestroyMe if the bullet is a piercing bullet and hit something that doesn't have a Damagable

        static void DestroyMeHandlePierce(ProjectileHit instance, HitInfo hit)
        {
            if (!(instance.ownWeapon?.GetComponent<Gun>()?.GetData().pierce ?? false))
            {
                // the bullet is not a piercing bullet
                instance.InvokeMethod("DestroyMe");
            }
            else if (hit.transform?.GetComponent<HealthHandler>() is null && hit.collider?.GetComponentInParent<Damagable>() is null)
            {
                // the bullet is a piercing bullet and hit a wall
                instance.InvokeMethod("DestroyMe");
            }
            if (instance.gameObject.GetComponentInChildren<InstantKillHitEffect>() is InstantKillHitEffect effect)
            {
                UnityEngine.Debug.Log("Found!");
                effect.active = false;
            }
            instance.damage *= .75f;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var m_DestroyMe = ExtensionMethods.GetMethodInfo(typeof(ProjectileHit), "DestroyMe");
            var m_DestroyMeHandlePierce = ExtensionMethods.GetMethodInfo(typeof(ProjectileHit_Patch_RPCA_DoHit_Pierce), nameof(DestroyMeHandlePierce));

            foreach (var instruction in instructions)
            {
                if (instruction.Calls(m_DestroyMe))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Call, m_DestroyMeHandlePierce);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}