using UnityEngine;

namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// An interface for implementing a common <see cref="Rect"/>.
    /// </summary>
    public interface IDimensions
    {
        /// <summary>
        /// The position and width of height of a <see cref="Control"/>.
        /// </summary>
        Rect Dimensions { get; set; }
    }
}
