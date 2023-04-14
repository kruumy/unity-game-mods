using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Fixed
{
    public class Group : NestedControl, IContent, IDimensions
    {
        /// <inheritdoc/>
        public GUIContent Content { get; set; } = new GUIContent("");

        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(10, 10, 300, 300);

        /// <inheritdoc/>
        public override void Draw()
        {
            GUI.BeginGroup(Dimensions, Content);
            base.Draw();
            GUI.EndGroup();
        }
    }
}
