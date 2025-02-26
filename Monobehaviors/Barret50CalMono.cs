using ModdingUtils.RoundsEffects;
using Photon.Pun;
using Ported_FFC.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ported_FFC.Monobehaviors
{
    public class Barret50CalMono : MonoBehaviour
    {
        private Player _player;
        private InstantKillHitEffect _hitEffect;

        private void Awake()
        {
            if (_player == null) _player = gameObject.GetComponent<Player>();
        }

        private void Start()
        {
            _hitEffect = _player.gameObject.AddComponent<InstantKillHitEffect>();
        }

        private void Update()
        {
            var extendedMags = _player.data.stats.GetAdditionalData().extendedMags;
            gameObject.GetComponent<Holding>().holdable.GetComponent<Gun>().GetComponentInChildren<GunAmmo>().maxAmmo =
                extendedMags;
        }

        private void OnDestroy()
        {
            Destroy(_hitEffect);
        }
    }

    public class InstantKillHitEffect : HitEffect
    {
        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null)
        {
            if (damagedPlayer == null) return;
            if (damagedPlayer.data.stats.remainingRespawns > 0)
            {
                damagedPlayer.data.view.RPC("RPCA_Die_Phoenix", RpcTarget.All, new object[]
                {
                    damage
                });
            }
            else
            {
                damagedPlayer.data.view.RPC("RPCA_Die", RpcTarget.All, new object[]
                {
                    damage
                });
            }
        }
    }
}
