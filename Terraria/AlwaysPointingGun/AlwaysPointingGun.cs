using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AlwaysPointingGun
{
    public class AlwaysPointingGun : ModPlayer
    {
        public override void PostUpdate()
        {
            base.PostUpdate();

            if ( this.Player.whoAmI == Main.myPlayer && this.Player.HeldItem != null && this.Player.HeldItem.useStyle == Terraria.ID.ItemUseStyleID.Shoot && this.Player.itemAnimation > 0 )
            {
                Vector2 vector = this.Player.RotatedRelativePoint(this.Player.MountedCenter, false, true);
                float num8 = (float)Main.mouseX + Main.screenPosition.X - vector.X;
                float num9;
                if ( this.Player.gravDir < 0 ) // IF FLIPPED
                {
                    num9 = Main.screenHeight - (float)Main.mouseY + Main.screenPosition.Y - vector.Y;
                }
                else
                {
                    num9 = (float)Main.mouseY + Main.screenPosition.Y - vector.Y;
                }

                this.Player.itemRotation = (float)Math.Atan2((double)(num9 * (float)this.Player.direction), (double)(num8 * (float)this.Player.direction)) - this.Player.fullRotation;
            }
        }
    }
}
