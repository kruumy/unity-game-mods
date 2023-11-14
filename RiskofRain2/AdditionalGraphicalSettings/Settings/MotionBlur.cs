using AddFoVSettings;
using System;

namespace AdditionalGraphicalSettings.Settings
{
    public sealed class MotionBlur : ForEachEffectCheckBoxGraphicalSetting<UnityEngine.Rendering.PostProcessing.MotionBlur>
    {
        public MenuSlider SampleCount { get; }
        public MenuSlider ShutterAngle { get; }
        public MotionBlur() : base(false, "Motion Blur", string.Empty)
        {
            SampleCount = new MenuSlider(10, 100, 0, true, "Motion Blur Sample Count", "The quality of the motion blur.", SubPanel.Graphics, true);
            ShutterAngle = new MenuSlider(270f, 5760f, 0f, true, "Motion Blur Shutter Angle", "The amount of motion blur.", SubPanel.Graphics, true);
            SampleCount.OnSliderChanged += SampleCount_OnSliderChanged;
            ShutterAngle.OnSliderChanged += ShutterAngle_OnSliderChanged;
        }
        protected override void OnCameraSpawn()
        {
            base.OnCameraSpawn();
            SampleCount_OnSliderChanged(SampleCount.GetValue());
            ShutterAngle_OnSliderChanged(ShutterAngle.GetValue());
        }

        private void ShutterAngle_OnSliderChanged( float newValue )
        {
            ForEachEffect(( UnityEngine.Rendering.PostProcessing.MotionBlur setting ) =>
            {
                setting.shutterAngle.value = newValue;
                setting.shutterAngle.overrideState = true;
            });
        }

        private void SampleCount_OnSliderChanged( float newValue )
        {
            ForEachEffect(( UnityEngine.Rendering.PostProcessing.MotionBlur setting ) =>
            {
                setting.sampleCount.value = Convert.ToInt32(newValue);
                setting.shutterAngle.overrideState = true;
            });
        }
    }
}
