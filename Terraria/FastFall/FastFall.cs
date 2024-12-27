using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FastFall
{
    public class FastFall : ModPlayer
    {
        public override void Load()
        {
            Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.DrawPlayerFull += On_LegacyPlayerRenderer_DrawPlayerFull;
        }

        private void On_LegacyPlayerRenderer_DrawPlayerFull( Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.orig_DrawPlayerFull orig, Terraria.Graphics.Renderers.LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, Player drawPlayer )
        {
            if ( !Config.Instance.DisableShadowEffect && Delay <= 0 && !drawPlayer.pulley && Math.Abs(drawPlayer.velocity.Y) > 0 && drawPlayer.controlDown && !drawPlayer.controlJump && !drawPlayer.mount.Active )
            {
                for ( int k = 0; k < 3; k++ )
                {
                    // TODO figure out why head not on shadow
                    self.DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[ k ], drawPlayer.shadowRotation[ k ], drawPlayer.shadowOrigin[ k ], 0.5f + 0.2f * (float)k, 1f);
                }
            }
            orig.Invoke(self, camera, drawPlayer);
        }

        private int Delay = 0;
        public override void PreUpdateMovement()
        {
            Delay--;
            if ( Delay <= 0 && !Player.pulley && Player.controlDown && !Player.controlJump && !Player.mount.Active )
            {

                Vector2 newVelocity = Player.velocity;

                if ( this.Player.gravDir < 0 ) // IF FLIPPED
                {
                    if ( !(Player.velocity.Y < 0) )
                    {
                        newVelocity.Y = -0.1f; // make player start falling
                    }
                }
                else
                {
                    if ( !(Player.velocity.Y > 0) )
                    {
                        newVelocity.Y = 0.1f; // make player start falling
                    }
                }

                newVelocity.Y *= Config.Instance.FastFallVelocityMultiplier;
                Player.velocity = newVelocity;

                if ( !Config.Instance.DisableGroundPounding && Player.velocity.Y > 0 )
                {
                    Rectangle playerRect = Player.getRect();
                    playerRect.Inflate(15, 15);
                    foreach ( NPC npc in Main.npc )
                    {
                        if ( npc.active && !npc.dontTakeDamage && !npc.friendly && (npc.aiStyle != 112 || npc.ai[ 2 ] <= 1f) && Player.CanNPCBeHitByPlayerOrPlayerProjectile(npc, null) )
                        {
                            Rectangle npcRect = npc.getRect();
                            if ( playerRect.Center.Y < npcRect.Center.Y && playerRect.Intersects(npcRect) && (npc.noTileCollide || Player.CanHit(npc)) )
                            {
                                if ( Player.whoAmI == Main.myPlayer )
                                {
                                    Player.velocity.Y = -Config.Instance.PlayerKnockBackOnGroundPound;
                                    Player.ApplyDamageToNPC(npc, (int)Player.GetTotalDamage<MeleeDamageClass>().ApplyTo(Config.Instance.BaseEnemyDamageOnGroundPound), Config.Instance.EnemyKnockBackOnGroundPound, Player.direction, false);
                                    Player.GiveImmuneTimeForCollisionAttack(4);
                                    Delay = Config.Instance.TickFastFallDelayOnGroundPound;
                                }

                            }
                        }
                    }
                }

            }
        }
        public override void Unload()
        {
            Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.DrawPlayerFull -= On_LegacyPlayerRenderer_DrawPlayerFull;
        }
    }
}