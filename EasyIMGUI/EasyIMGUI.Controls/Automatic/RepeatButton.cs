using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.RepeatButton(GUIContent, GUIStyle, GUILayoutOption[])"/>.
    /// </summary>
    public class RepeatButton : Base.Button, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            if (GUILayout.RepeatButton(Content, LayoutOptions))
            {
                Invoke_OnButtonPressed();
            }
        }
    }
}
