using HarmonyLib;
using ModdingUtils.RoundsEffects;
using Photon.Pun;
using Ported_FFC_Classic.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;
using WeaponsManager;

namespace Ported_FFC_Classic.Monobehaviors
{
    public class Barret50CalMono : MonoBehaviour
    {
        private RightLeftMirrorSpring ammoPos;
        private GameObject barrel;
        private GameObject barrel2;
        private Gun gun;
        private InstantKillHitEffect hitEffect;
        private static GameObject _InstantKillObj = null;
        private GameObject oldIcon;
        private string oldName;
        private Player player;
        private GameObject scope;
        private WeaponManager weaponManager;

        public static GameObject InstantKillObj // Object that holds the instant kill effect
        {
            get
            {
                if (_InstantKillObj == null)
                {
                    _InstantKillObj = new GameObject("A_InstantKill", typeof(InstantKillHitEffect));
                    DontDestroyOnLoad(_InstantKillObj);
                }
                return _InstantKillObj;
            }
        }

        private void Awake()
        {
            player = gameObject.GetComponentInParent<Player>();
            weaponManager = player.gameObject.GetComponent<WeaponManager>();
            gun = weaponManager.weapons[0];
        }

        private void Start()
        {
            if (player.data.view.IsMine)
            { // Change "Pistol" icon in weaponManager to the rifle
                oldName = weaponManager.names[0];
                weaponManager.names[0] = "Barrett .50 Cal";
                oldIcon = weaponManager.icons[0];
                oldIcon.SetActive(false);
                GameObject newIcon = PFFCC.assets.LoadAsset<GameObject>("I_Barrett50Cal");
                weaponManager.icons[0] = Instantiate(newIcon, oldIcon.transform.parent);
                Destroy(newIcon);
                weaponManager.UpdateIcons();
            }

            // Adjust weapon visuals
            barrel = gun.transform.Find("Spring").Find("Barrel").gameObject;
            barrel2 = Instantiate(barrel, gun.transform.Find("Spring"));
            scope = Instantiate(barrel, gun.transform.Find("Spring"));
            scope.transform.localScale = new Vector3(0.65f, 0.2f, 1f);
            scope.transform.localPosition = new Vector3(-0.3f, 0.6f, 0f);
            RightLeftMirrorSpring scopeSpring = scope.GetComponent<RightLeftMirrorSpring>();
            scopeSpring.leftPos = new Vector3(0.3f, 0.6f, 0f);

            scope.GetComponent<BoxCollider2D>().enabled = false;

            Vector3 originalScale = barrel.transform.localScale;
            barrel2.transform.localScale = Vector3.Scale(new Vector3(3.5f, 1f, 1f), originalScale);
            RightLeftMirrorSpring mirrorSpring = barrel2.GetComponent<RightLeftMirrorSpring>();
            Vector3 originalLeftPos = mirrorSpring.leftPos;
            Vector3 originalRightPos = (Vector3)mirrorSpring.GetFieldValue("rightPos");
            mirrorSpring.leftPos = new Vector3(0.15f, 1.1f, 0f);
            barrel2.transform.localPosition = new Vector3(-0.15f, 1.1f, 0f);

            GameObject ammo = gun.transform.Find("Spring").Find("Ammo").gameObject;
            ammo.transform.localPosition = new Vector3(-0.2f, 0, 0);
            ammoPos = ammo.AddComponent<RightLeftMirrorSpring>();
            ammoPos.leftPos = new Vector3(0.2f, 0, 0);

            // Add the instant kill action
            gun.ShootPojectileAction += OnShootProjectileAction;
        }

        public void OnShootProjectileAction(GameObject obj)
        {
            if (weaponManager.activeWeapon == 0) // Don't apply instant kill to alternate weapons
            {
                TrailRenderer trailRenderer = obj.GetComponentInChildren<TrailRenderer>();
                trailRenderer.startColor = Color.red;
                trailRenderer.endColor = Color.red;
                Instantiate(InstantKillObj, obj.transform);
            }
        }

        private void Update()
        {
            if (weaponManager.activeWeapon == 0) // Enforce stats for rifle
            {
                player.data.weaponHandler.gun.GetComponentInChildren<GunAmmo>().maxAmmo = player.data.stats.GetAdditionalData().extendedMags;
                player.data.weaponHandler.gun.bursts = 1;
                player.data.weaponHandler.gun.numberOfProjectiles = 1;
            }
        }

        private void OnDestroy()
        {
            Destroy(hitEffect);
            Destroy(scope);
            Destroy(barrel2);
            Destroy(ammoPos);
            WeaponManager weaponManager = player.gameObject.GetComponent<WeaponManager>();
            weaponManager.names[0] = oldName;
            weaponManager.icons[0] = oldIcon;
            oldIcon.SetActive(true);
            gun.transform.Find("Spring").Find("Ammo").transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public class InstantKillHitEffect : RayHitEffect
    {
        public bool active = true;
        public override HasToReturn DoHitEffect(HitInfo hit)
        {
            if (!hit.transform || !active)
            { 
                return HasToReturn.canContinue;
            }
            if (hit.transform.GetComponent<Player>() is Player damagedPlayer && damagedPlayer != null)
            {
                if (damagedPlayer.data.stats.remainingRespawns > 0)
                {
                    damagedPlayer.data.view.RPC("RPCA_Die_Phoenix", RpcTarget.All, new object[]
                    {
                    hit.normal
                    });
                }
                else
                {
                    damagedPlayer.data.view.RPC("RPCA_Die", RpcTarget.All, new object[]
                    {
                    hit.normal
                    });
                }
            }
            return HasToReturn.canContinue;
        }
    }
}
