using BepInEx;
using UnityEngine;

namespace FOVModifier
{
    [BepInPlugin("kruumy.FOVModifier", "FOVModifier", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public void LateUpdate()
        {
            if ( Input.GetKeyDown(KeyCode.Minus) )
            {
                Camera.main.fieldOfView -= 5;
            }
            else if ( Input.GetKeyDown(KeyCode.Equals) )
            {
                Camera.main.fieldOfView += 5;
            }
        }
    }
}
