using AdditionalGraphicalSettings.MenuAPI;
using UnityEngine;

namespace AdditionalGraphicalSettings.Settings
{
    public class RenderSettings : Setting
    {
        public MenuSlider Scaling { get; }
        public MenuSlider FarClipPlane { get; }
        public RenderSettings() : base("Render", SubPanel.Video)
        {
            Scaling = CreateMenuSliderWithResetToDefault(1, 1, 0.01f, false, "Render Resolution", string.Empty, true, ( float newValue ) =>
            {
                RoR2.CameraResolutionScaler.SetResolutionScale(newValue);
            });
            FarClipPlane = CreateMenuSliderWithResetToDefault(4000, 5000, 1, true, "Render Distance", "Changes the cameras far clip plane. May lead to less objects being drawn so maybe better performace. Could also be used to control fog distance.", true, ( float newValue ) =>
            {
                Camera.main.farClipPlane = newValue;
            });
        }
    }
}
