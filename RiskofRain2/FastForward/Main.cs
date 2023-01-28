using BepInEx;
using System;
using UnityEngine;

namespace FastForward
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginAuthor = "kruumy";
        public const string PluginName = "FastForward";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginVersion = "1.0.0";
        public void OnGUI()
        {
            if (Time.timeScale > 1)
            {
                GUI.Label(new Rect(0, 0, 100, 100), Math.Round(Time.timeScale, 2).ToString());
            }
        }
        public void LateUpdate()
        {
            if (Input.GetKey(KeyCode.V))
            {
                if (Time.timeScale <= 4f)
                {
                    Time.timeScale *= 1.01f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.V))
            {
                Time.timeScale = 1f;
            }
        }
    }
}
