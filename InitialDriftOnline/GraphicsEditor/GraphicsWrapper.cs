using UnityEngine.Rendering.PostProcessing;

namespace GraphicsEditor
{
    public static class GraphicsWrapper
    {
        public static bool AmbientOcclusion
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<AmbientOcclusion>().active;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<AmbientOcclusion>().active = value;
        }

        public static bool Bloom
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<Bloom>().active;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<Bloom>().active = value;
        }

        public static bool ChromaticAberration
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<ChromaticAberration>().active;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<ChromaticAberration>().active = value;
        }

        public static bool DepthOfField
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<DepthOfField>().active;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<DepthOfField>().active = value;
        }

        public static bool MotionBlur
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<MotionBlur>().active;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessSettings<MotionBlur>().active = value;
        }
    }
}
