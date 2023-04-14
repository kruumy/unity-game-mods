using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.Label(GUIContent, GUIStyle, GUILayoutOption[])"/>.
    /// </summary>
    public class Label : Base.Label, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>

        public override void Draw()
        {
            GUILayout.Label(Content, LayoutOptions);
        }
    }
}
