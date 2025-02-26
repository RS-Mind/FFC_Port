using System;
using System.Collections;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnboundLib.GameModes;

namespace Ported_FFC.Extensions
{
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public bool JokesOnYou;
        public bool hasAdaptiveSizing;
        public float adaptiveMovementSpeed;
        public float adaptiveGravity;
        public float healing;
        public float damageReduction;
        public int extendedMags;
        public int kingOfFools;

        public CharacterStatModifiersAdditionalData()
        {
            JokesOnYou = false;
            hasAdaptiveSizing = false;
            adaptiveMovementSpeed = 0f;
            adaptiveGravity = 0f;
            healing = 1;
            damageReduction = 0;
            extendedMags = 1;
            kingOfFools = 0;
        }
    }


    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> Data =
                new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers statModifiers)
        {
            return Data.GetOrCreateValue(statModifiers);
        }

        public static void AddData(this CharacterStatModifiers statModifiers, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                Data.Add(statModifiers, value);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }


        [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
        private class CharacterStatModifiersPatchResetStats
        {
            private static void Prefix(CharacterStatModifiers __instance)
            {
                var additionalData = __instance.GetAdditionalData();
                additionalData.JokesOnYou = false;
                additionalData.adaptiveMovementSpeed = 0f;
                additionalData.adaptiveGravity = 0f;
                additionalData.hasAdaptiveSizing = false;
                additionalData.healing = 1;
                additionalData.damageReduction = 0;
                additionalData.extendedMags = 1;
                additionalData.kingOfFools = 0;
            }
        }
    }
}
