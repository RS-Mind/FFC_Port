using BepInEx;
using HarmonyLib;
using Jotunn.Utils;
using UnityEngine;

namespace Ported_FFC
{

    [BepInDependency("root.classes.manager.reborn")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class PFFC : BaseUnityPlugin
    {
        private const string ModId = "root.port.fluxxfield.fluxxfieldscards";
        private const string ModName = "Port of FFC";
        private const string Version = "1.3";
        public const string ModInitials = "PFFC";
        internal static AssetBundle assets;
        public static PFFC instance { get; private set; }

        private void Awake() 
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            assets = AssetUtils.LoadAssetBundleFromResources("pffcart", typeof(PFFC).Assembly);
            if (assets == null)
            {
                UnityEngine.Debug.Log("Failed to load PFFC asset bundle");
            }
            assets.LoadAsset<GameObject>("CardHolder").GetComponent<CardHolder>().RegisterCards();
        }

        private void Start()
        {
            instance = this;
        }

    }
}
