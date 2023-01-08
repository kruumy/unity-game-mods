namespace GraphicsEditor
{
    public static class PostProcessingWrapper
    {
        public static bool AmbientOcclusion
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.ambientOcclusion.enabled;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.ambientOcclusion.enabled = value;
        }

        public static bool Bloom
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.bloom.enabled;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.bloom.enabled = value;
        }

        public static bool ChromaticAberration
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.chromaticAberration.enabled;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.chromaticAberration.enabled = value;
        }

        public static bool DepthOfField
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.depthOfField.enabled;
            set => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.depthOfField.enabled = value;
        }

        public static bool MotionBlur
        {
            get => RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.motionBlur.enabled;
            set
            {
                RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.motionBlur.enabled = value;
                UnityEngine.PostProcessing.MotionBlurModel.Settings settings = RCC_SceneManager.Instance.activeMainCamera.get_PostProcessingBehaviour().profile.motionBlur.settings;
                settings.shutterAngle = 180;
            }
        }

        public static void Enable()
        {
            RCC_SceneManager.Instance.activeMainCamera.EnablePostProcessing();
        }
    }
}
