using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Fixed
{
    public class ScrollView : Base.ScrollView, IDimensions
    {
        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(0, 0, 0, 0);

        /// <inheritdoc/>
        public override void Draw()
        {
            // TODO: check this
            Position = GUI.BeginScrollView(Dimensions, Position, Dimensions, AlwaysShowHorizontal, AlwaysShowVertical);
            base.Draw();
            GUI.EndScrollView();
        }
    }
}
