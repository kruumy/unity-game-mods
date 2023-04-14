using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.VerticalScrollbar(float, float, float, float, GUIStyle, GUILayoutOption[])"/>.
    /// </summary>
    public class VerticalScrollbar : Base.Scrollbar, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.VerticalScrollbar(Value, Size, Maximum, Minimum, LayoutOptions);
        }
    }
}
