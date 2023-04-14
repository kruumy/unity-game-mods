namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a selection grid.
    /// </summary>
    public abstract class SelectionGrid : Toolbar
    {
        /// <summary>
        /// The amount of buttons to be placed along the the X-Axis.
        /// </summary>
        public int Width { get; set; } = 2;
    }
}
