using MelonLoader;

namespace GraphicsEditor
{
    public static class Preferences
    {
        public static MelonPreferences_Category MainCatagory { get; } = MelonPreferences.CreateCategory(nameof(GraphicsEditor));
        public static MelonPreferences_Entry<bool> MotionBlur { get; } = MainCatagory.CreateEntry(nameof(MotionBlur), false);
        public static MelonPreferences_Entry<bool> Bloom { get; } = MainCatagory.CreateEntry(nameof(Bloom), false);
        public static MelonPreferences_Entry<bool> ChromaticAberration { get; } = MainCatagory.CreateEntry(nameof(ChromaticAberration), false);
        public static MelonPreferences_Entry<bool> AmbientOcclusion { get; } = MainCatagory.CreateEntry(nameof(AmbientOcclusion), false);
        public static MelonPreferences_Entry<bool> DepthOfField { get; } = MainCatagory.CreateEntry(nameof(DepthOfField), false);


        public static void Save()
        {
            MotionBlur.Value = GraphicsWrapper.MotionBlur;
            Bloom.Value = GraphicsWrapper.Bloom;
            ChromaticAberration.Value = GraphicsWrapper.ChromaticAberration;
            AmbientOcclusion.Value = GraphicsWrapper.AmbientOcclusion;
            DepthOfField.Value = GraphicsWrapper.DepthOfField;
            MelonPreferences.Save();
        }

        public static void Load()
        {
            GraphicsWrapper.MotionBlur = MotionBlur.Value;
            GraphicsWrapper.Bloom = Bloom.Value;
            GraphicsWrapper.ChromaticAberration = ChromaticAberration.Value;
            GraphicsWrapper.AmbientOcclusion = AmbientOcclusion.Value;
            GraphicsWrapper.DepthOfField = DepthOfField.Value;
            MelonPreferences.Load();
        }
    }
}
