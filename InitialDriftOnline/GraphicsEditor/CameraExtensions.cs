using UnityEngine.Rendering.PostProcessing;

namespace GraphicsEditor
{
    public static class CameraExtensions
    {
        public static PostProcessVolume get_PostProcessVolume(this UnityEngine.Camera camera)
        {
            return camera.GetComponent<PostProcessVolume>();
        }
        public static PostProcessLayer get_PostProcessLayer(this UnityEngine.Camera camera)
        {
            return camera.GetComponent<PostProcessLayer>();
        }
        public static void EnablePostProcessing(this UnityEngine.Camera camera)
        {
            camera.get_PostProcessLayer().enabled = true;
            camera.get_PostProcessVolume().enabled = true;
        }
        public static void DisableAllPostProcessProfiles(this PostProcessVolume ppv)
        {
            ppv.profile.settings.ForEach(p => p.active = false);
        }
        public static void DisableAllPostProcessProfiles(this UnityEngine.Camera camera)
        {
            camera.get_PostProcessVolume().profile.settings.ForEach(p => p.active = false);
        }
        public static T get_PostProcessSettings<T>(this UnityEngine.Camera camera)
        {
            foreach (PostProcessEffectSettings item in camera.get_PostProcessVolume().profile.settings)
            {
                if (item is T t)
                {
                    return t;
                }
            }
            return default;
        }
    }
}
