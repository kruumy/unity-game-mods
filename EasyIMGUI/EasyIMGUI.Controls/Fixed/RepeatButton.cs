using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Fixed
{
    public class RepeatButton : Base.Button, IDimensions
    {
        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(0, 0, 0, 0);

        /// <inheritdoc/>
        public override void Draw()
        {
            if (GUI.RepeatButton(Dimensions, Content))
            {
                Invoke_OnButtonPressed();
            }
        }
    }
}
