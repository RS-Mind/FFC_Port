using HarmonyLib;
using Ported_FFC.Monobehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponsManager;

namespace Ported_FFC.Extensions
{
    public class PFFCStats : MonoBehaviour // Used to apply custom stats to unity cards
    {
        public bool JokesOnYou = false;
        public bool hasAdaptiveSizing = false;
        public bool pierce = false;
        public float adaptiveMovementSpeed = 0f;
        public float adaptiveGravity = 0f;
        public float healing = 1f;
        public float damageReduction = 0f;
        public int extendedMags = 0;
        public int kingOfFools = 0;

        public void Apply(Player player)
        {
            player.data.stats.GetAdditionalData().JokesOnYou = JokesOnYou ? true : player.data.stats.GetAdditionalData().JokesOnYou;
            player.data.stats.GetAdditionalData().hasAdaptiveSizing = hasAdaptiveSizing ? true : player.data.stats.GetAdditionalData().hasAdaptiveSizing;
            player.data.stats.GetAdditionalData().adaptiveMovementSpeed += adaptiveMovementSpeed;
            player.data.stats.GetAdditionalData().adaptiveGravity += adaptiveGravity;
            player.data.stats.GetAdditionalData().healing *= healing;
            player.data.stats.GetAdditionalData().damageReduction += damageReduction;
            player.data.stats.GetAdditionalData().extendedMags += extendedMags;
            player.data.stats.GetAdditionalData().kingOfFools += kingOfFools;
            try
            {
                player.gameObject.GetComponent<WeaponManager>().weapons[0].GetData().pierce = pierce ? true : player.gameObject.GetComponent<WeaponManager>().weapons[0].GetData().pierce;
            }
            catch 
            {
                UnityEngine.Debug.LogWarning("PFFC Failed to find Weapon Manager on the player");
            }
        }
    }

    [HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
    public class ApplyPlayerStatsPatch
    {
        static void Postfix(ApplyCardStats __instance, Player ___playerToUpgrade)
        {
            if (__instance.GetComponent<PFFCStats>() is PFFCStats stats)
            {
                stats.Apply(___playerToUpgrade);
            }
        }
    }
}