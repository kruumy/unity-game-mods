using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.Window(int, Rect, GUI.WindowFunction, GUIContent, GUIStyle, GUILayoutOption[])"/>.
    /// </summary>
    public class Window : Base.Window, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        public bool AutoResizeHeight { get; set; } = false;

        /// <inheritdoc/>
        public override void Draw()
        {
            Dimensions = GUILayout.Window(ID, Dimensions, WindowFunction, Content, LayoutOptions);
            if (AutoResizeHeight)
            {
                Dimensions = new Rect(Dimensions.x, Dimensions.y, Dimensions.width, 0);
            }
        }
    }
}
