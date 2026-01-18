using System.Collections;
using System.Collections.Generic;
using UnboundLib.GameModes;
using UnboundLib;
using UnityEngine;
using Ported_FFC_Classic.Extensions;

namespace Ported_FFC_Classic.Monobehaviors
{
    public class SizeMattersMono : MonoBehaviour
    {
        private float lastHealthPercet;
        private float sizeDelta = 0f;
        private float movementSpeedDelta = 0f;
        private float gravtyDelta = 0f;
        private Player _player;
        private void Start()
        {
            if (_player == null) _player = gameObject.GetComponentInParent<Player>();
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, (gm) => reset());
        }
        private void Update()
        {
            if (_player == null || !_player.data.stats.GetAdditionalData().hasAdaptiveSizing) return;
            float healthPrecent = _player.data.health / _player.data.maxHealth;
            if (Mathf.Clamp(healthPrecent, 0f, 1f) == lastHealthPercet) return;
            lastHealthPercet = healthPrecent;
            _player.data.stats.movementSpeed -= movementSpeedDelta;
            _player.data.stats.gravity -= gravtyDelta;
            _player.data.stats.sizeMultiplier -= sizeDelta;

            movementSpeedDelta = _player.data.stats.movementSpeed * (_player.data.stats.GetAdditionalData().adaptiveMovementSpeed - (_player.data.stats.GetAdditionalData().adaptiveMovementSpeed * healthPrecent));
            gravtyDelta = -_player.data.stats.gravity * (_player.data.stats.GetAdditionalData().adaptiveGravity - (_player.data.stats.GetAdditionalData().adaptiveGravity * healthPrecent));
            sizeDelta = (_player.data.stats.sizeMultiplier * (Mathf.Clamp(healthPrecent, 0f, 1f) / 2 + 0.5f)) - _player.data.stats.sizeMultiplier;


            _player.data.stats.movementSpeed += movementSpeedDelta;
            _player.data.stats.gravity += gravtyDelta;
            _player.data.stats.sizeMultiplier += sizeDelta;

            _player.data.stats.InvokeMethod("ConfigureMassAndSize");
        }
        public IEnumerator reset()
        {
            try
            {
                _player.data.stats.movementSpeed -= movementSpeedDelta;
                _player.data.stats.gravity -= gravtyDelta;
                _player.data.stats.sizeMultiplier -= sizeDelta;
                movementSpeedDelta = 0;
                gravtyDelta = 0;
                sizeDelta = 0;
            }
            catch { }
            yield break;
        }
    }
}
