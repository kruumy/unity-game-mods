using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.PasswordField(string, char, int, GUIStyle, GUILayoutOption[])"/>.
    /// </summary>
    public class PasswordField : Base.PasswordField, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.PasswordField(Value, MaskCharacter, MaxLength, LayoutOptions);
        }
    }
}
