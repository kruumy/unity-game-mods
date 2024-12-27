using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NakedDash
{
    public class NakedDash : ModPlayer
    {
        public override void Load()
        {
            Terraria.On_Player.DashMovement += On_Player_DashMovement;
        }
        private int Delay = 0;
        private void On_Player_DashMovement( Terraria.On_Player.orig_DashMovement orig, Terraria.Player self )
        {
            if ( !Config.Instance.DisableNakedDash && self.dashType == 0 )
            {
                Delay--;
                if ( Delay <= 0 )
                {
                    PlayerExtentions.DoCommonDashHandle(self, out int dir, out bool dashing);

                    if ( dashing && Math.Abs(self.velocity.X) <= Config.Instance.DashPower )
                    {
                        self.velocity.X = Config.Instance.DashPower * dir;
                        Delay = Config.Instance.DashDelay;
                        for ( int num21 = 0; num21 < 20; num21++ )
                        {
                            int num22 = Dust.NewDust(new Vector2(self.position.X, self.position.Y), self.width, self.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                            Dust dust = Main.dust[ num22 ];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            Main.dust[ num22 ].velocity *= 0.2f;
                            Main.dust[ num22 ].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        }
                    }
                }
            }
            else
            {
                orig.Invoke(self);
            }
        }

        public override void Unload()
        {
            Terraria.On_Player.DashMovement -= On_Player_DashMovement;
        }

    }
}
