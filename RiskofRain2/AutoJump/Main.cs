using BepInEx;
using BepInEx.Logging;
using System;
using System.Linq;
using UnityEngine;

namespace AutoJump
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginAuthor = "kruumy";
        public const string PluginName = "AutoJump";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginVersion = "1.0.0";
        public static ManualLogSource logger;
        public void Awake()
        {
            logger = this.Logger;

        }
        public void Update()
        {




            if (Input.GetKey(KeyCode.Space))
            {
                try
                {
                    RoR2.CharacterBody player = UnityEngine.Object.FindObjectsOfType<RoR2.CharacterBody>().First(c => c.isPlayerControlled);
                    if (player != null && player.characterMotor.isGrounded)
                    {
                        player.characterMotor.Jump(1, 1, false);
                    }
                }
                catch (InvalidOperationException)
                {

                }

            }
        }
    }
}
