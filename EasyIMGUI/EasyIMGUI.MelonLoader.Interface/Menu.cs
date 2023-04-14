using EasyIMGUI.Controls.Base;
using MelonLoader;
using System;
using System.Collections.Generic;

namespace EasyIMGUI.MelonLoader.Interface
{
    /// <summary>
    /// Class containing the implementation of a common use case of <see cref="EasyIMGUI"/>
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Invoked on Close()
        /// </summary>
        public event EventHandler OnClosed;

        /// <summary>
        /// Invoked on Open()
        /// </summary>
        public event EventHandler OnOpen;

        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="EasyIMGUI.Controls"/> to <see cref="Draw"/>
        /// </summary>
        public List<Control> Controls { get; } = new List<Control>();

        /// <summary>
        /// Determines of the <see cref="Menu"/> is opened or not.
        /// </summary>
        public bool IsOpen { get; private set; } = false;

        /// <summary>
        /// Unsubscribes the <see cref="Draw"/> method to <see cref="MelonEvents.OnGUI"/>.
        /// </summary>
        public void Close()
        {
            if (IsOpen)
            {
                MelonEvents.OnGUI.Unsubscribe(Draw);
                IsOpen = false;
                OnClosed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // TODO: throw exeption here
            }
        }

        /// <summary>
        /// Subscribes the <see cref="Draw"/> method to <see cref="MelonEvents.OnGUI"/>.
        /// </summary>
        public void Open()
        {
            if (!IsOpen)
            {
                MelonEvents.OnGUI.Subscribe(Draw);
                IsOpen = true;
                OnOpen?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // TODO: throw exeption here
            }
        }

        /// <summary>
        /// Toggles between <see cref="Open"/> and <see cref="Close"/> on each invoke.
        /// </summary>
        public void Toggle()
        {
            if (!IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        private void Draw()
        {
            Controls.ForEach(c => c.Draw());
        }
    }
}
