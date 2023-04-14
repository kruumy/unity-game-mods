using EasyIMGUI.Controls.Base;
using System;

namespace EasyIMGUI.Controls.Extra
{
    /// <summary>
    /// A <see cref="Control"/> containing events <see cref="OnPreDraw"/> and <see cref="OnPostDraw"/> that allow you to run your own <see cref="Action"/> before or after controls are drawn.
    /// </summary>
    public class Event : NestedControl
    {
        public event EventHandler OnPreDraw;
        public event EventHandler OnPostDraw;
        public override void Draw()
        {
            OnPreDraw?.Invoke(this, EventArgs.Empty);
            base.Draw();
            OnPostDraw?.Invoke(this, EventArgs.Empty);
        }
    }
}
