using ClassesManagerReborn.Util;
using Ported_FFC_Classic.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Ported_FFC_Classic.Cards.Jester
{
    public class WayOfTheJesterMono : MonoBehaviour
    {
        private const float Damage = 0.05f;
        private float deltaDamage = 0f; 
        private const float MovementSpeed = 0.01f;
        private float deltaMovementSpeed = 0f;
        private const float ProjectileSpeed = 0.03f;
        private float deltaProjectileSpeed = 0f;
        private int _bounces;
        private Gun _gun;
        private Player _player;
        private int _previousBounces = 0;
        private CharacterStatModifiers _stats;

        public void Start()
        {
            if (_player == null) _player = gameObject.GetComponentInParent<Player>();
        }
        private void Update()
        {
            if (_player == null) return;
            _stats = _player.data.stats;
            _gun = _player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            var bounce_cap = 25;
            _bounces = Mathf.Clamp(_gun.reflects, 0, bounce_cap);
            if (_bounces == _previousBounces) return;
            _previousBounces = _bounces;

            _stats.movementSpeed -= deltaMovementSpeed;
            _gun.damage -= deltaDamage;
            _gun.projectileSpeed -= deltaProjectileSpeed;

            deltaMovementSpeed = _stats.movementSpeed * MovementSpeed * _bounces;
            deltaDamage = _gun.damage * Damage * _bounces;
            deltaProjectileSpeed = _gun.projectileSpeed * _bounces * ProjectileSpeed;

            _stats.movementSpeed += deltaMovementSpeed;
            
            _gun.damage += deltaDamage;

            _gun.projectileSpeed += deltaProjectileSpeed;
        }
    }

}
