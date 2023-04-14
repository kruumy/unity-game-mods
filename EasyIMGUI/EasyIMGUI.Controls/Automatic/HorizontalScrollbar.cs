using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.HorizontalScrollbar(float, float, float, float, GUILayoutOption[])"/>.
    /// </summary>
    public class HorizontalScrollbar : Base.Scrollbar, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.HorizontalScrollbar(Value, Size, Minimum, Maximum, LayoutOptions);
        }
    }
}
