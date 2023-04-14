using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Extra
{
    public class SelectorItem : IContent
    {
        /// <inheritdoc/>
        public GUIContent Content { get; set; } = new GUIContent("");

        public Control Control { get; set; }
    }
}
