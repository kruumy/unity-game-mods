using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace NakedDash
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static Config Instance;

        [DefaultValue(false)]
        public bool DisableNakedDash;

        [DefaultValue(9f)]
        [Range(0f, 20f)]
        public float DashPower;

        [DefaultValue(30)]
        public int DashDelay;

        public override void OnLoaded()
        {
            Config.Instance = this;
        }
    }
}
