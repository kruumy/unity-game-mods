using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Fixed
{
    public class Texture : Control, IDimensions
    {
        public bool AlphaBlend { get; set; } = false;

        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(0, 0, 0, 0);

        public UnityEngine.Texture Image { get; set; }
        public float ImageAspect { get; set; } = 0f;
        public ScaleMode ScaleMode { get; set; } = ScaleMode.StretchToFill;

        /// <inheritdoc/>
        public override void Draw()
        {
            GUI.DrawTexture(Dimensions, Image, ScaleMode, AlphaBlend, ImageAspect);
        }
    }
}
