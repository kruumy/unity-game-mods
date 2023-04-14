using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Fixed
{
    public class VerticalScrollbar : Base.Scrollbar, IDimensions
    {
        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(0, 0, 0, 0);

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUI.VerticalScrollbar(Dimensions, Value, Size, Maximum, Minimum);
        }
    }
}
