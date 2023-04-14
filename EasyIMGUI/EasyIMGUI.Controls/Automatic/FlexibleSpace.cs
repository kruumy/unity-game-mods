using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Automatic
{
    /// <summary>
    /// A <see cref="Control"/> containing the implementation of <see cref="GUILayout.FlexibleSpace"/>.
    /// </summary>
    public class FlexibleSpace : Control
    {
        /// <inheritdoc/>
        public override void Draw()
        {
            GUILayout.FlexibleSpace();
        }
    }
}
