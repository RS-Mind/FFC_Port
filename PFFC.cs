using BepInEx;
using HarmonyLib;
using Jotunn.Utils;
using ToggleCardsCategories;
using UnityEngine;

namespace Ported_FFC
{

    [BepInDependency("root.classes.manager.reborn")]
    [BepInDependency("com.aalund13.rounds.toggle_cards_categories", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class PFFC : BaseUnityPlugin
    {
        private const string ModId = "root.port.fluxxfield.fluxxfieldscards";
        private const string ModName = "Port of FFC";
        private const string Version = "1.3.10";
        public const string ModInitials = "PFFC";
        internal static AssetBundle assets;
        public static PFFC instance { get; private set; }

        private void Awake() 
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            assets = AssetUtils.LoadAssetBundleFromResources("pffc", typeof(PFFC).Assembly);
            if (assets == null)
            {
                UnityEngine.Debug.Log("Failed to load PFFC asset bundle");
            }
            ToggleCardsCategoriesManager.instance.RegisterCategories(ModInitials);
            assets.LoadAsset<GameObject>("CardHolder").GetComponent<CardHolder>().RegisterCards();
        }

        private void Start()
        {
            instance = this;
        }

    }
}
