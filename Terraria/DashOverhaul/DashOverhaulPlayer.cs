using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DashOverhaul
{
    internal class DashOverhaulPlayer : ModPlayer
    {
        public bool enable = false;
        private int dashCooldown = -1;
        public const int DASH_MAX_COOLDOWN = 60;
        public const int DASH_LENGTH = 10;
        public bool IsDashReady => dashCooldown < 0;
        public bool IsDashing => dashCooldown >= DASH_MAX_COOLDOWN - DASH_LENGTH;
        public bool DashJustEnded => dashCooldown == DASH_MAX_COOLDOWN - DASH_LENGTH - 1;
        public bool DashCoolDownJustEnded => dashCooldown == 0;
        private Vector2 dashDirection = Vector2.Zero;
        public const float DASH_SPEED = 30f;
        private Vector2 preDashVelocity = Vector2.Zero;
        public override void ResetEffects()
        {
            enable = false;
        }

        public override void PreUpdateMovement()
        {
            if (!enable)
                return;

            if (dashCooldown > -1)
                dashCooldown--;

            if (DashOverhaul.DashKey.JustPressed && IsDashReady)
            {
                preDashVelocity = Player.velocity;
                dashCooldown = DASH_MAX_COOLDOWN;
                SoundEngine.PlaySound(SoundID.Item74, Player.position);
                dashDirection = GetInputDirection();
                Player.immuneTime = DASH_LENGTH;
                Main.SetCameraLerp(0.1f, DASH_LENGTH);
            }

            if (IsDashing)
            {
                Player.immune = true;
                Player.velocity = dashDirection * DASH_SPEED;

                for (int i = 0; i < 2; i++)
                {
                    Vector2 position = Player.Center - dashDirection * 10f;
                    Vector2 velocity = -dashDirection * Main.rand.NextFloat(1f, 3f) + new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));

                    Dust dust = Dust.NewDustPerfect(position, DustID.Smoke, velocity, 150, default, 7.5f);
                    dust.noGravity = true;
                    dust.fadeIn = 0.5f;
                }
            }
            else if (DashJustEnded)
            {
                Player.immune = false;
                Player.velocity = dashDirection * preDashVelocity.Length();
            }
            else if (DashCoolDownJustEnded)
            {
                SoundEngine.PlaySound(SoundID.Item4, Player.position);
                
            }
        }

        public Vector2 GetInputDirection()
        {
            Vector2 dir = Vector2.Zero;

            if (Player.controlLeft)
                dir.X -= 1f;
            if (Player.controlRight)
                dir.X += 1f;
            if (Player.controlUp)
                dir.Y -= 1f;
            if (Player.controlDown)
                dir.Y += 1f;
            if (dir == Vector2.Zero)
                dir.X = Player.direction;

            return dir;
        }
    }
}