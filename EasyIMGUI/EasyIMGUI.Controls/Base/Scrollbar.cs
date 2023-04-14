namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a scrollbar.
    /// </summary>
    public abstract class Scrollbar : Slider
    {
        /// <summary>
        /// The size of the <see cref="Scrollbar"/>.
        /// </summary>
        public float Size { get; set; } = 25;
    }
}
