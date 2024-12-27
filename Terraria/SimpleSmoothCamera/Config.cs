using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SimpleSmoothCamera
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static Config Instance;

        [DefaultValue(0.05f)]
        [Range(0f, 1f)]
        public float MouseInfluence;


        [DefaultValue(false)]
        public bool EnableMouseInfluenceOnUse;

        [DefaultValue(0.05f)]
        [Range(0f, 1f)]
        public float MouseInfluenceOnUse;

        [DefaultValue(false)]
        public bool EnableMouseInfluenceOnRightClick;

        [DefaultValue(0.05f)]
        [Range(0f, 1f)]
        public float MouseInfluenceOnRightClick;



        [DefaultValue(0.9f)]
        [Range(0f, 1f)]
        public float FollowSpeed;

        [DefaultValue(0.05f)]
        [Range(0f, 1f)]
        public float DampingFactor;

        public override void OnLoaded()
        {
            Config.Instance = this;
        }
    }
}
