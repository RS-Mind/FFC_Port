using ClassesManagerReborn.Util;
using ModdingUtils.RoundsEffects;
using Photon.Pun;
using Ported_FFC_Classic.Extensions;
using Ported_FFC_Classic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Networking;
using UnityEngine;
using Random = System.Random;

namespace Ported_FFC_Classic.Cards.Jester
{
    public class KingOfFoolsHitSurfaceEffect : HitSurfaceEffect
    {

        private readonly Random _rng = new Random();
        private const int BaseChance = 15;
        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            Player player = gameObject.GetComponentInParent<Player>();
            var multiplier = player.data.stats.GetAdditionalData().kingOfFools;
            var role = _rng.Next(1, 101);

            if (multiplier == 0 || role > multiplier * BaseChance || !PhotonNetwork.IsMasterClient) return;
            NetworkingManager.RPC(typeof(KingOfFoolsHitSurfaceEffect), nameof(DoKingOfFoolsRPC), position, normal, velocity, player.playerID);
        }

        [UnboundRPC]
        public static void DoKingOfFoolsRPC(Vector2 position, Vector2 normal, Vector2 velocity, int playerID)
        {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerID);

            Gun gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();

            Gun newGun = player.gameObject.AddComponent<KingOfFoolsGun>();

            SpawnBulletsEffect effect = player.gameObject.AddComponent<SpawnBulletsEffect>();
            // set the position and direction to fire
            Vector2 parallel = ((Vector2)Vector3.Cross(Vector3.forward, normal)).normalized;
            List<Vector3> positions = GetPositions(position, normal, parallel);
            List<Vector3> directions = GetDirections(position, positions);
            effect.SetPositions(positions);
            effect.SetDirections(directions);
            effect.SetNumBullets(1);
            effect.SetTimeBetweenShots(0f);
            effect.SetInitialDelay(0f);

            // copy private gun stats over and reset a few public stats
            SpawnBulletsEffect.CopyGunStats(gun, newGun);

            newGun.spread = 0.2f;
            newGun.numberOfProjectiles = 1;
            newGun.projectiles = (from e in Enumerable.Range(0, newGun.numberOfProjectiles) from x in newGun.projectiles select x).ToList().Take(newGun.numberOfProjectiles).ToArray();
            newGun.damage = UnityEngine.Mathf.Clamp(newGun.damage / 2f, 0.5f, float.MaxValue);
            newGun.projectileSpeed = UnityEngine.Mathf.Clamp(velocity.magnitude / 100f, 0.1f, 1f);
            newGun.damageAfterDistanceMultiplier = 1f;
            List<ObjectsToSpawn> objectsToSpawns = newGun.objectsToSpawn.ToList();
            objectsToSpawns.Add(PreventRecursion.stopRecursionObjectToSpawn);
            newGun.objectsToSpawn = objectsToSpawns.ToArray();
            newGun.projectileColor = new Color(255f / 255f, 221f / 255f, 0f / 255f);
            // set the gun of the spawnbulletseffect
            effect.SetGun(newGun);
        }

        private static List<Vector3> GetPositions(Vector2 position, Vector2 normal, Vector2 parallel)
        {
            List<Vector3> res = new List<Vector3>() { };

            res.Add(position + 0.2f * normal + 0.1f  * parallel);

            return res;
        }

        private static List<Vector3> GetDirections(Vector2 position, List<Vector3> shootPos)
        {
            List<Vector3> res = new List<Vector3>() { };

            foreach (Vector3 shootposition in shootPos)
            {
                res.Add(((Vector2)shootposition - position).normalized);
            }

            return res;
        }

        public class KingOfFoolsGun : Gun
        {

        }
    }
}
