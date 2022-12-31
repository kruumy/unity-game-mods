using MelonLoader;
using UnityEngine;

namespace SaveEditor
{
    public class Main : MelonMod
    {
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                if (!Menu.Menu.IsOpen)
                {
                    Menu.Menu.Enable();
                }
                else
                {
                    Menu.Menu.Disable();
                }
            }
        }
    }
}
