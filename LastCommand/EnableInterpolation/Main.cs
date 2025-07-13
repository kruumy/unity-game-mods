using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnableInterpolation
{
    [BepInEx.BepInPlugin("kruumy.EnableInterpolation", "EnableInterpolation", "1.0.0")]
    public class Main : BepInEx.BaseUnityPlugin
    {
        private MethodInfo _CameraFollowFixedUpdate;
        public MethodInfo CameraFollowFixedUpdate
        {
            get
            {
                if ( _CameraFollowFixedUpdate == null )
                {
                    _CameraFollowFixedUpdate = typeof(CameraFollow).GetMethod("FixedUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                return _CameraFollowFixedUpdate;
            }
        }
        private CameraFollow _CameraFollowInstance;
        public CameraFollow CameraFollowInstance
        {
            get
            {
                if ( _CameraFollowInstance == null )
                {
                    _CameraFollowInstance = UnityEngine.Object.FindObjectOfType<CameraFollow>();
                }
                return _CameraFollowInstance;
            }
        }
        public void Awake()
        {
            Harmony harmony = new Harmony("com.yourname.fixedupdatepatch");
            harmony.PatchAll(typeof(HarmonyLib.Patches));
        }
        public void FixedUpdate()
        {
            foreach ( var rb in UnityEngine.Object.FindObjectsOfType<Rigidbody2D>() )
            {
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            }
        }

        public void LateUpdate()
        {
            CameraFollowFixedUpdate.Invoke(CameraFollowInstance, null);
        }
    }
}

