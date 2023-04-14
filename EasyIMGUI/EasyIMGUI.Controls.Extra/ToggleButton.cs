using EasyIMGUI.Controls.Automatic;
using UnityEngine;

namespace EasyIMGUI.Controls.Extra
{
    public class ToggleButton : Toggle
    {
        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.Toggle(Value, Content, "button", LayoutOptions);
        }
    }
}
