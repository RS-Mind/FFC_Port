using HarmonyLib;
using Ported_FFC.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
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
            if (damagingPlayer == (Player)__instance.GetFieldValue("player") || damagingPlayer == null) return;
            float res = ((CharacterStatModifiers)__instance.GetFieldValue("stats")).GetAdditionalData().damageReduction;
            if (res > 0)
            {
                damage -= damage * res;
            }
        }
    }
}
