using MelonLoader;

namespace GraphicsEditor
{
    public static class Preferences
    {
        public static MelonPreferences_Category MainCatagory { get; } = MelonPreferences.CreateCategory(nameof(GraphicsEditor));
        public static MelonPreferences_Entry<bool> AmbientOcclusion { get; } = MainCatagory.CreateEntry(nameof(AmbientOcclusion), false);
        public static MelonPreferences_Entry<bool> Bloom { get; } = MainCatagory.CreateEntry(nameof(Bloom), false);
        public static MelonPreferences_Entry<bool> DepthOfField { get; } = MainCatagory.CreateEntry(nameof(DepthOfField), false);
        public static MelonPreferences_Entry<bool> ChromaticAberration { get; } = MainCatagory.CreateEntry(nameof(ChromaticAberration), false);
        public static MelonPreferences_Entry<bool> MotionBlur { get; } = MainCatagory.CreateEntry(nameof(MotionBlur), false);


        public static void Save()
        {
            MotionBlur.Value = PostProcessingWrapper.MotionBlur;
            Bloom.Value = PostProcessingWrapper.Bloom;
            ChromaticAberration.Value = PostProcessingWrapper.ChromaticAberration;
            AmbientOcclusion.Value = PostProcessingWrapper.AmbientOcclusion;
            DepthOfField.Value = PostProcessingWrapper.DepthOfField;
            MelonPreferences.Save();
        }
        public static void Load()
        {
            PostProcessingWrapper.MotionBlur = MotionBlur.Value;
            PostProcessingWrapper.Bloom = Bloom.Value;
            PostProcessingWrapper.ChromaticAberration = ChromaticAberration.Value;
            PostProcessingWrapper.AmbientOcclusion = AmbientOcclusion.Value;
            PostProcessingWrapper.DepthOfField = DepthOfField.Value;
            MelonPreferences.Load();
        }
    }
}
