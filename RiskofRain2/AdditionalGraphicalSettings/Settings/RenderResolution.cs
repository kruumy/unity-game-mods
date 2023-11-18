using AdditionalGraphicalSettings.MenuAPI;

namespace AdditionalGraphicalSettings.Settings
{
    public class RenderResolution : Setting
    {
        public MenuSlider Scaling { get; }
        public RenderResolution()
        {
            Scaling = new MenuSlider(1, 1, 0.01f, false, "Render Resolution", string.Empty, SubPanel.Video, true, ( float newValue ) =>
            {
                RoR2.CameraResolutionScaler.SetResolutionScale(newValue);
            });
        }
    }
}
