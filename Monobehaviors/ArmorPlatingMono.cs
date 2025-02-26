using ClassesManagerReborn.Util;
using Ported_FFC.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnboundLib.Networking;
using System.Linq;
using HarmonyLib;
using UnboundLib.GameModes;

namespace Ported_FFC.Cards.Juggernaut
{
    public class ArmorPlatingMono : MonoBehaviour
    {
        public List<bool> reflect_rolls;
        public int indx;

        public void Start()
        {
            GameModeManager.AddHook(GameModeHooks.HookPointStart, (gm) => SyncOdds(this.GetComponent<Player>().playerID));
        }

        public IEnumerator SyncOdds(int playerID)
        {
            if (!PhotonNetwork.IsMasterClient) yield break;
            bool[] bools = new bool[1000];
            for (int i = 0; i < 1000; ++i) bools[i] = UnityEngine.Random.Range(0,4) == 0? true : false;
            NetworkingManager.RPC(typeof(ArmorPlatingMono), nameof(OddsRPCA), playerID, bools);
            yield break;
        }

        [UnboundRPC]
        public static void OddsRPCA(int playerID, bool[] bools)
        {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerID);
            ArmorPlatingMono armorPlating = player.GetComponent<ArmorPlatingMono>();
            if(armorPlating == null) return;
            armorPlating.reflect_rolls = bools.ToList();
            armorPlating.indx = 0;
        }
        public bool Reflect()
        {
            return reflect_rolls[indx++ % 1000];
        }

    }


    [Serializable]
    [HarmonyPatch(typeof(ProjectileHit), "Hit")]
    class ProjectileHitPatchHit
    {
        // disable friendly fire or self damage and hit effects if that setting is enabled
        private static bool Prefix(ProjectileHit __instance, HitInfo hit, bool forceCall)
        {
            HealthHandler healthHandler = null;
            if (hit.transform)
            {
                healthHandler = hit.transform.GetComponent<HealthHandler>();
            }
            if (healthHandler)
            {
                Player hitPlayer = healthHandler.GetComponent<Player>();
                // if the hit player is not null
                if (hitPlayer != null && hitPlayer.GetComponent<ArmorPlatingMono>() != null && hitPlayer.GetComponent<ArmorPlatingMono>().Reflect())
                {
                    __instance.GetComponent<ProjectileHit>().RemoveOwnPlayerFromPlayersHit();
                    __instance.GetComponent<ProjectileHit>().AddPlayerToHeld(healthHandler);
                    __instance.GetComponent<MoveTransform>().velocity *= -1f;
                    __instance.transform.position += __instance.GetComponent<MoveTransform>().velocity * TimeHandler.deltaTime;
                    __instance.GetComponent<RayCastTrail>().WasBlocked();
                    if (__instance.destroyOnBlock)
                    {
                        __instance.InvokeMethod("DestroyMe");
                    }
                    __instance.sinceReflect = 0f;
                    return false;
                }
            }
            return true;
        }
    }
}
