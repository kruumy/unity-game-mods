using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.BeginVertical(GUIContent, GUIStyle, GUILayoutOption[])"/> and <see cref="GUILayout.EndVertical()"/>
    /// </summary>
    public class Vertical : NestedControl, ILayoutOptions, IContent
    {
        /// <inheritdoc/>
        public GUIContent Content { get; set; } = new GUIContent("");

        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            GUILayout.BeginVertical(Content, GUIStyle.none, LayoutOptions);
            base.Draw();
            GUILayout.EndVertical();
        }
    }
}
