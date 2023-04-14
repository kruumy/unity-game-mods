using UnityEngine;

namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a window.
    /// </summary>
    public abstract class Window : NestedControl, IDimensions, IContent
    {
        /// <inheritdoc/>
        public GUIContent Content { get; set; } = new GUIContent("");

        /// <inheritdoc/>
        public Rect Dimensions { get; set; } = new Rect(0, 0, 300, 300);

        /// <summary>
        /// The id of the window, is set to a random integer by default.
        /// </summary>
        public int ID { get; set; } = new System.Random().Next();

        /// <summary>
        /// Determines if a window's task bar is dragable and can change the <see cref="Dimensions"/>.
        /// </summary>
        public bool IsDragable { get; set; } = true;

        /// <summary>
        /// The <see cref="GUI.WindowFunction"/>.
        /// </summary>
        /// <param name="windowID">The <see cref="ID"/> being passed through.</param>
        protected void WindowFunction(int windowID)
        {
            base.Draw();
            if (IsDragable)
            {
                GUI.DragWindow(new Rect(0, 0, Dimensions.width, 20));
            }
        }
    }
}
