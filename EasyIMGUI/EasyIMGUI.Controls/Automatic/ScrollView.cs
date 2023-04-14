using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.BeginScrollView(Vector2, GUIStyle, GUILayoutOption[])"/> and <see cref="GUILayout.EndScrollView"/>.
    /// </summary>
    public class ScrollView : Base.ScrollView, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            Position = GUILayout.BeginScrollView(Position, AlwaysShowHorizontal, AlwaysShowVertical, LayoutOptions);
            base.Draw();
            GUILayout.EndScrollView();
        }
    }
}
