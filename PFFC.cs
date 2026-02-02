using BepInEx;
using HarmonyLib;
using Jotunn.Utils;
using ToggleCardsCategories;
using UnityEngine;

namespace Ported_FFC_Classic
{

    [BepInDependency("root.classes.manager.reborn")]
    [BepInDependency("com.aalund13.rounds.toggle_cards_categories", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class PFFCC : BaseUnityPlugin
    {
        private const string ModId = "root.port.fluxxfield.fluxxfieldscardsclassic";
        private const string ModName = "Port of FFC - Classic";
        private const string Version = "4.5.1";
        public const string ModInitials = "PFFCC";
        internal static AssetBundle assets;
        public static PFFCC instance { get; private set; }

        private void Awake() 
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            assets = AssetUtils.LoadAssetBundleFromResources("pffcart", typeof(PFFCC).Assembly);
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
