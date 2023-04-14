using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Fixed
{
    public class Clip : NestedControl, IDimensions
    {
        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(10, 10, 300, 300);

        public Vector2 RenderOffset { get; set; } = new Vector2(0, 0);
        public bool ResetOffset { get; set; } = false;
        public Vector2 ScrollOffset { get; set; } = new Vector2(0, 0);

        /// <inheritdoc/>
        public override void Draw()
        {
            GUI.BeginClip(Dimensions, ScrollOffset, RenderOffset, ResetOffset);
            base.Draw();
            GUI.EndClip();
        }
    }
}
