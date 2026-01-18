using HarmonyLib;
using Ported_FFC_Classic.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace Ported_FFC_Classic.Patches
{
    [Serializable]
    [HarmonyPatch(typeof(HealthHandler), "Heal")]
    public class JuggernautPatch
    {
        private static void Prefix(HealthHandler __instance, ref float healAmount)
        {
            Player player = (Player)__instance.GetFieldValue("player");
            healAmount *= player.data.stats.GetAdditionalData().healing;
        }
    }
}
