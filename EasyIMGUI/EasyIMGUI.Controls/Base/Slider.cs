namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a slider.
    /// </summary>
    public abstract class Slider : ValueControl<float>
    {
        /// <summary>
        /// The maximum value of the <see cref="Slider"/>.
        /// </summary>
        public float Maximum { get; set; } = 10;

        /// <summary>
        /// The minimum value of the <see cref="Slider"/>.
        /// </summary>
        public float Minimum { get; set; } = 0;
    }
}
