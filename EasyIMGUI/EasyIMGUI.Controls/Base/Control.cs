namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// The absolute base class for a gui control.
    /// Defines <see cref="Draw"/>.
    /// </summary>
    public abstract class Control
    {
        /// <summary>
        /// Invokes the underlying <see cref="UnityEngine.GUILayout"/> or <see cref="UnityEngine.GUI"/> element.
        /// The most root <see cref="Control"/> should invoke <see cref="Draw"/> in the Unity OnGUI method.
        /// </summary>
        public abstract void Draw();
    }
}
