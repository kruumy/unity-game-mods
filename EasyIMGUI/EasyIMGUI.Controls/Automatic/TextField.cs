using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.TextField(string, int, GUIStyle, GUILayoutOption[])"/>.
    /// </summary>
    public class TextField : Base.TextControl, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.TextField(Value, MaxLength, LayoutOptions);
        }
    }
}
