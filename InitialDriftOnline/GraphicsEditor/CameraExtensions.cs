using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace GraphicsEditor
{
    public static class CameraExtensions
    {
        private static PostProcessVolume lastppv;
        public static PostProcessVolume get_PostProcessVolume(this UnityEngine.Camera camera)
        {
            if (camera.GetComponent<PostProcessVolume>() == null)
            {
                // Not working
                PostProcessVolume res = camera.gameObject.AddComponent<PostProcessVolume>();
                res.isGlobal = true;
                res.profile = lastppv.profile;
                res.sharedProfile = lastppv.sharedProfile;
                res.gameObject.layer = LayerMask.NameToLayer("Post Processing");
                return res;
            }
            else
            {
                lastppv = camera.GetComponent<PostProcessVolume>();
                return camera.GetComponent<PostProcessVolume>();
            }
        }
        private static PostProcessLayer lastppl;
        public static PostProcessLayer get_PostProcessLayer(this UnityEngine.Camera camera)
        {
            if (camera.GetComponent<PostProcessLayer>() == null)
            {
                // Not working
                PostProcessLayer res = camera.gameObject.AddComponent<PostProcessLayer>();
                camera.tag = "MainCamera";
                res.volumeTrigger = camera.transform;
                res.volumeLayer = LayerMask.GetMask("Post Processing");
                return res;
            }
            else
            {
                lastppl = camera.GetComponent<PostProcessLayer>();
                return camera.GetComponent<PostProcessLayer>();
            }
        }
        public static void EnablePostProcessing(this UnityEngine.Camera camera)
        {
            camera.get_PostProcessVolume().enabled = true;
            camera.get_PostProcessLayer().enabled = true;
        }
        public static void DisableAllPostProcessProfiles(this PostProcessVolume ppv)
        {
            ppv.profile.settings.ForEach(p => p.active = false);
        }
        public static void DisableAllPostProcessProfiles(this UnityEngine.Camera camera)
        {
            camera.get_PostProcessVolume().profile.settings.ForEach(p => p.active = false);
        }
        public static T get_PostProcessSettings<T>(this UnityEngine.Camera camera) where T : PostProcessEffectSettings
        {
            foreach (PostProcessEffectSettings item in camera.get_PostProcessVolume().profile.settings)
            {
                if (item is T t)
                {
                    return t;
                }
            }
            camera.get_PostProcessVolume().profile.settings.Add((T)Activator.CreateInstance(typeof(T)));
            return (T)camera.get_PostProcessVolume().profile.settings.Last();
        }
    }
}
