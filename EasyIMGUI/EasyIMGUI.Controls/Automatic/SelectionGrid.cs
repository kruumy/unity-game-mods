using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.SelectionGrid(int, GUIContent[], int, GUIStyle, GUILayoutOption[])"/>.
    /// </summary>
    public class SelectionGrid : Base.SelectionGrid, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.SelectionGrid(Value, Contents.ToArray(), Width, LayoutOptions);
        }
    }
}
