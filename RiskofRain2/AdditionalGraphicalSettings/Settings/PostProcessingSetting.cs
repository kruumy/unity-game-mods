using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace AdditionalGraphicalSettings.Settings
{
    public abstract class PostProcessingSetting
    {
        protected static PostProcessVolume Volume { get; } = CreateBaseVolume();

        private static PostProcessVolume CreateBaseVolume()
        {
            GameObject ppGameObject = new GameObject($"{Main.PluginAuthor}_{Main.PluginName}_Volume");
            Object.DontDestroyOnLoad(ppGameObject);
            ppGameObject.layer = RoR2.LayerIndex.postProcess.intVal;
            PostProcessVolume postProcessVolume = ppGameObject.AddComponent<PostProcessVolume>();
            Object.DontDestroyOnLoad(postProcessVolume);
            postProcessVolume.enabled = true;
            postProcessVolume.isGlobal = true;
            postProcessVolume.weight = 1f;
            postProcessVolume.priority = float.MaxValue;
            postProcessVolume.sharedProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
            postProcessVolume.sharedProfile.name = $"{Main.PluginAuthor}_{Main.PluginName}_Profile";
            Object.DontDestroyOnLoad(postProcessVolume.sharedProfile);
            postProcessVolume.profile = postProcessVolume.sharedProfile;
            return postProcessVolume;
        }
    }
}
