using MelonLoader;
using UnityEngine;

namespace FreeMoney
{
    public class Main : MelonMod
    {
        SRAdminTools AdminTools => UnityEngine.Object.FindObjectOfType<SRAdminTools>();
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                if (AdminTools != null)
                {
                    AdminTools.GiveMoney();
                    MelonLogger.Msg("Given $1000");
                }
                else
                {
                    MelonLogger.Error("AdminTools == null");
                }
            }
        }
    }
}
