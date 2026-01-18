using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ported_FFC_Classic.Utils
{
    public class PreventRecursion
    {
        private static GameObject _stopRecursion = null;

        internal static GameObject stopRecursion
        {
            get
            {
                if (PreventRecursion._stopRecursion != null) { return PreventRecursion._stopRecursion; }
                else
                {
                    _stopRecursion = new GameObject("StopRecursion", typeof(StopRecursion));
                    UnityEngine.GameObject.DontDestroyOnLoad(_stopRecursion);

                    return PreventRecursion._stopRecursion;
                }
            }
            set { }
        }
        internal static ObjectsToSpawn stopRecursionObjectToSpawn
        {
            get
            {
                ObjectsToSpawn obj = new ObjectsToSpawn() { };
                obj.AddToProjectile = PreventRecursion.stopRecursion;

                return obj;
            }
            set { }
        }
    }
    public class DestroyOnUnparentAfterInitialized : MonoBehaviour
    {
        private static bool initialized = false;
        private bool isOriginal = false;

        void Start()
        {
            if (initialized) { isOriginal = true; }
        }
        void LateUpdate()
        {
            if (this.isOriginal) { return; }
            else if (this.gameObject.transform.parent == null) {Destroy(gameObject); }
        }
    }
}
