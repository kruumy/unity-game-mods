using EasyIMGUI.Controls.Automatic;
using UnityEngine;

namespace EasyIMGUI.Controls.Extra
{
    public class EasySelectionGrid : Selector, ILayoutOptions
    {
        /// <inheritdoc/>
        public LayoutOptions LayoutOptions { get; set; } = new LayoutOptions();

        public int Width { get; set; } = 2;

        /// <inheritdoc/>
        public override void Draw()
        {
            Value = GUILayout.SelectionGrid(Value, Items.Contents, Width, LayoutOptions);
            base.Draw();
        }
    }
}
