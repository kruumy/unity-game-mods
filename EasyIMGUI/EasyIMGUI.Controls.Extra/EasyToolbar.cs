using EasyIMGUI.Controls.Automatic;
using UnityEngine;

namespace EasyIMGUI.Controls.Extra
{
    public class EasyToolbar : Selector, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.Toolbar(Value, Items.Contents, LayoutOptions);
            base.Draw();
        }
    }
}
