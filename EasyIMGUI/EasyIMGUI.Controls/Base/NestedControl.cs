using System.Collections.Generic;

namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// Base class for a <see cref="Control"/> that contains other <see cref="EasyIMGUI.Controls"/>.
    /// </summary>
    public abstract class NestedControl : Control
    {
        /// <summary>
        /// The list of <see cref="EasyIMGUI.Controls"/> that are nested.
        /// Each <see cref="Control"/>'s <see cref="Control.Draw"/> method in invoked in <see cref="Draw"/>
        /// </summary>
        public List<Control> Controls { get; } = new List<Control>();

        /// <summary>
        /// Invokes each <see cref="Controls"/>'s <see cref="Control.Draw"/> method.
        /// If inherinting, wrap your funtionality around <see cref="Draw"/>
        /// </summary>
        public override void Draw()
        {
            Controls.ForEach(c => c.Draw());
        }
    }
}
