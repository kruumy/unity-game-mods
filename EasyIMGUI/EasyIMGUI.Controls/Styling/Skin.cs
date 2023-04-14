using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Styling
{
    public class Skin : NestedControl
    {
        public GUISkin GUISkin { get; set; } = new GUISkin();

        /// <inheritdoc/>
        public override void Draw()
        {
            GUI.skin = GUISkin;
            base.Draw();
            GUI.skin = null;
        }
    }
}
