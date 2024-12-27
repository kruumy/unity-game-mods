using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SimpleSmoothCamera
{
    public class SimpleSmoothCamera : Mod
    {
        public override void Load()
        {
            Terraria.On_Main.DoDraw_UpdateCameraPosition += On_Main_DoDraw_UpdateCameraPosition;
        }

        private Vector2 targetPosition;

        private void On_Main_DoDraw_UpdateCameraPosition( On_Main.orig_DoDraw_UpdateCameraPosition orig )
        {
            if ( Main.gameMenu || Main.netMode == NetmodeID.Server )
            {
                return;
            }

            Vector2 targetCenter = Main.LocalPlayer.getRect().Center.ToVector2();

            Vector2 mouseOffset = Main.MouseWorld - targetCenter;

            if ( Main.LocalPlayer.controlUseTile && Config.Instance.EnableMouseInfluenceOnRightClick )
            {
                mouseOffset *= Config.Instance.MouseInfluenceOnRightClick;
            }
            else if ( Main.LocalPlayer.controlUseItem && Config.Instance.EnableMouseInfluenceOnUse )
            {
                mouseOffset *= Config.Instance.MouseInfluenceOnUse;
            }
            else
            {
                mouseOffset *= Config.Instance.MouseInfluence;
            }

            targetPosition = Vector2.Lerp(targetPosition, targetCenter + mouseOffset, Config.Instance.FollowSpeed);

            Main.screenPosition.X = Damp(Main.screenPosition.X, targetPosition.X - Main.screenWidth * 0.5f, Config.Instance.DampingFactor, 1f / 60f);
            Main.screenPosition.Y = Damp(Main.screenPosition.Y, targetPosition.Y - Main.screenHeight * 0.5f, Config.Instance.DampingFactor, 1f / 60f);

            // TODO add support for the binoculars and scope and anything else that modify camera
        }
        public static float Damp( float source, float destination, float smoothing, float dt )
        {
            return MathHelper.Lerp(source, destination, 1f - MathF.Pow(smoothing, dt));
        }

        public override void Unload()
        {
            Terraria.On_Main.DoDraw_UpdateCameraPosition -= On_Main_DoDraw_UpdateCameraPosition;
        }
    }
}