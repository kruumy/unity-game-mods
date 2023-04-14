using UnityEngine;

namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// An interface for implementing a common <see cref="GUIContent"/>.
    /// </summary>
    public interface IContent
    {
        /// <summary>
        /// The <see cref="GUIContent"/> to be used in a <see cref="Control"/>.
        /// </summary>
        GUIContent Content { get; set; }
    }
}
