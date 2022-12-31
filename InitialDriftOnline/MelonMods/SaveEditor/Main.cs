using MelonLoader;
using UnityEngine;

namespace SaveEditor
{
    public class Main : MelonMod
    {
        private bool IsMenuOpen = false;
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                if (!IsMenuOpen)
                {
                    MelonEvents.OnGUI.Subscribe(Menu.Menu.Draw);
                    IsMenuOpen = true;
                }
                else
                {
                    MelonEvents.OnGUI.Unsubscribe(Menu.Menu.Draw);
                    IsMenuOpen = false;
                }
            }
        }
    }
}
