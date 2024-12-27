using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ScrollZoom
{
    public class ScrollZoom : Mod
    {
        private const float ZoomSpeed = 0.1f;
        private const float ZoomIncrement = 0.05f;
        private float currentZoom = 1.0f;

        public override void Load()
        {
            Terraria.On_Main.DoDraw_UpdateCameraPosition += On_Main_DoDraw_UpdateCameraPosition;
            Terraria.On_Player.ScrollHotbar += On_Player_ScrollHotbar;
        }
        public override void Unload()
        {
            Terraria.On_Main.DoDraw_UpdateCameraPosition -= On_Main_DoDraw_UpdateCameraPosition;
            Terraria.On_Player.ScrollHotbar -= On_Player_ScrollHotbar;
        }
        private void On_Player_ScrollHotbar( On_Player.orig_ScrollHotbar orig, Player self, int Offset )
        {
            return;
        }

        private void On_Main_DoDraw_UpdateCameraPosition( On_Main.orig_DoDraw_UpdateCameraPosition orig )
        {
            orig.Invoke();
            if(!Main.gamePaused && !Main.gameMenu && !Main.playerInventory)
            {
                currentZoom += ZoomIncrement * Terraria.GameInput.PlayerInput.ScrollWheelDelta / 120;
                currentZoom = MathHelper.Clamp(currentZoom, 1.0f, 2.0f);
                Main.GameZoomTarget = MathHelper.Lerp(Main.GameZoomTarget, currentZoom, ZoomSpeed);
            }
            else
            {
                currentZoom = Main.GameZoomTarget;
            }
        }
    }
}
