using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Fixed
{
    public class HorizontalScrollbar : Base.Scrollbar, IDimensions
    {
        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(0, 0, 0, 0);

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUI.HorizontalScrollbar(Dimensions, Value, Size, Minimum, Maximum);
        }
    }
}
