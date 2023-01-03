using MelonLoader;

namespace CameraEditor
{
    public static class Preferences
    {
        public static MelonPreferences_Category MainCatagory { get; } = MelonPreferences.CreateCategory(nameof(CameraEditor));
        public static MelonPreferences_Entry<float> FieldOfView { get; } = MainCatagory.CreateEntry(nameof(FieldOfView), 55f);
        public static MelonPreferences_Entry<float> Distance { get; } = MainCatagory.CreateEntry(nameof(Distance), 5.7f);
        public static MelonPreferences_Entry<float> Height { get; } = MainCatagory.CreateEntry(nameof(Height), 0.9f);
        public static MelonPreferences_Entry<float> PitchAngle { get; } = MainCatagory.CreateEntry(nameof(PitchAngle), 5f);
        public static MelonPreferences_Entry<float> OffsetX { get; } = MainCatagory.CreateEntry(nameof(OffsetX), 0f);
        public static MelonPreferences_Entry<float> OffsetY { get; } = MainCatagory.CreateEntry(nameof(OffsetY), 0.5f);
        public static MelonPreferences_Entry<float> YawAngle { get; } = MainCatagory.CreateEntry(nameof(YawAngle), 0f);


        public static void Save()
        {
            FieldOfView.Value = CameraWrapper.FieldOfView;
            Distance.Value = CameraWrapper.Distance;
            Height.Value = CameraWrapper.Height;
            PitchAngle.Value = CameraWrapper.PitchAngle;
            OffsetX.Value = CameraWrapper.OffsetX;
            OffsetY.Value = CameraWrapper.OffsetY;
            YawAngle.Value = CameraWrapper.YawAngle;
            MelonPreferences.Save();
        }
        public static void Load()
        {
            MelonPreferences.Load();
            CameraWrapper.FieldOfView = FieldOfView.Value;
            CameraWrapper.Distance = Distance.Value;
            CameraWrapper.Height = Height.Value;
            CameraWrapper.PitchAngle = PitchAngle.Value;
            CameraWrapper.OffsetX = OffsetX.Value;
            CameraWrapper.OffsetY = OffsetY.Value;
            CameraWrapper.YawAngle = YawAngle.Value;
        }
        public static void ResetAllToDefault()
        {
            FieldOfView.ResetToDefault();
            Distance.ResetToDefault();
            Height.ResetToDefault();
            PitchAngle.ResetToDefault();
            OffsetX.ResetToDefault();
            OffsetY.ResetToDefault();
            YawAngle.ResetToDefault();
            MelonPreferences.Save();
            Load();
            MelonLogger.Msg("Reseted All Preferences");
        }
    }
}
