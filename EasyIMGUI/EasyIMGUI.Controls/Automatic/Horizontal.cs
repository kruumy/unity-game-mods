using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.BeginHorizontal(GUIContent, GUIStyle, GUILayoutOption[])"/> and <see cref="GUILayout.EndHorizontal"/>.
    /// </summary>
    public class Horizontal : NestedControl, ILayoutOptions, IContent
    {
        /// <inheritdoc/>
        public GUIContent Content { get; set; } = new GUIContent("");

        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            GUILayout.BeginHorizontal(Content, GUIStyle.none, LayoutOptions);
            base.Draw();
            GUILayout.EndHorizontal();
        }
    }
}
