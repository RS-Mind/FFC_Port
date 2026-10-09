using HarmonyLib;
using Ported_FFC.Extensions;
using System;
using UnboundLib;
using UnityEngine;

namespace Ported_FFC.Patches
{
    [Serializable]
    [HarmonyPatch(typeof(HealthHandler), "DoDamage")]
    public class Patch
    {
        private static void Prefix(HealthHandler __instance, ref Vector2 damage, Player damagingPlayer)
        {
            float res = ((CharacterStatModifiers)__instance.GetFieldValue("stats")).GetAdditionalData().damageReduction;
            if (res > 0)
            {
                if (res > 0.9f)
                {
                    res = 0.9f;
                }
                damage -= damage * res;
            }
        }
    }
}
