namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a text control.
    /// </summary>
    public abstract class TextControl : ValueControl<string>
    {
        /// <summary>
        /// The max length for the <see cref="TextControl"/>.
        /// </summary>
        public int MaxLength { get; set; } = int.MaxValue;
    }
}
