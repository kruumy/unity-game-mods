using UnityEngine;

namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a box.
    /// </summary>
    public abstract class Box : Control, IContent
    {
        /// <inheritdoc/>
        public GUIContent Content { get; set; } = new GUIContent("");
    }
}
