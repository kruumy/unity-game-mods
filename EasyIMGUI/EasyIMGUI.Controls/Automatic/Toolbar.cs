using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.Toolbar(int, GUIContent[], GUIStyle, GUI.ToolbarButtonSize, GUILayoutOption[])"/>.
    /// </summary>
    public class Toolbar : Base.Toolbar, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.Toolbar(Value, Contents.ToArray(), LayoutOptions);
        }
    }
}
