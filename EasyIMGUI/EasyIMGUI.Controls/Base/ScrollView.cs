using UnityEngine;

namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a scrollview.
    /// </summary>
    public abstract class ScrollView : NestedControl
    {
        /// <summary>
        /// Wether to show the horizontal scroll bar always.
        /// </summary>
        public bool AlwaysShowHorizontal = false;

        /// <summary>
        /// Wether to show the vertical scroll bar always.
        /// </summary>
        public bool AlwaysShowVertical = false;

        /// <summary>
        /// The scroll position.
        /// </summary>
        public Vector2 Position { get; set; } = new Vector2(0, 0);
    }
}
