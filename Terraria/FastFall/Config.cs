using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace FastFall
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static Config Instance;

        [DefaultValue(1.5f)]
        [Range(0f, 5f)]
        public float FastFallVelocityMultiplier;

        [DefaultValue(false)]
        public bool DisableShadowEffect;

        [DefaultValue(false)]
        public bool DisableGroundPounding;

        [DefaultValue(10f)]
        [Range(0f, 30f)]
        public float PlayerKnockBackOnGroundPound;

        [DefaultValue(9f)]
        [Range(0f, 30f)]
        public float EnemyKnockBackOnGroundPound;

        [DefaultValue(30f)]
        [Range(0f, 200f)]
        public float BaseEnemyDamageOnGroundPound;

        [DefaultValue(30)]
        public int TickFastFallDelayOnGroundPound;

        public override void OnLoaded()
        {
            Config.Instance = this;
        }
    }
}
