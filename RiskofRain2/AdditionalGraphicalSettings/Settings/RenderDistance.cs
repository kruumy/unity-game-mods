using AdditionalGraphicalSettings.MenuAPI;
using UnityEngine;

namespace AdditionalGraphicalSettings.Settings
{
    public class RenderDistance : Setting
    {
        public MenuSlider FarClipPlane { get; }
        public RenderDistance()
        {
            FarClipPlane = new MenuSlider(4000, 5000, 1, true, "Render Distance", "Changes the cameras far clip plane. May lead to less objects being drawn so maybe better performace. Could also be used to control fog distance.", SubPanel.Video, true, ( float newValue ) =>
            {
                Camera.main.farClipPlane = newValue;
            });
        }
    }
}
