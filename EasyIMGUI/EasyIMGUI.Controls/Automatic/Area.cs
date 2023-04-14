using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.BeginArea(Rect, GUIContent, GUIStyle)"/> and <see cref="GUILayout.EndArea"/>.
    /// </summary>
    public class Area : NestedControl, IDimensions, IContent
    {
        /// <inheritdoc/>
        public GUIContent Content { get; set; } = new GUIContent("");

        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(Screen.width / 2, Screen.height / 2, 300, 300);

        /// <inheritdoc/>
        public override void Draw()
        {
            GUILayout.BeginArea(Dimensions, Content);
            base.Draw();
            GUILayout.EndArea();
        }
    }
}
