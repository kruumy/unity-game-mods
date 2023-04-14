namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a password field.
    /// </summary>
    public abstract class PasswordField : TextControl
    {
        /// <summary>
        /// The character to be used to hide typed characters.
        /// </summary>
        public char MaskCharacter { get; set; } = '*';
    }
}
