using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace BlackBackgroundDuringBoss
{
	public class BlackBackgroundDuringBoss : Mod
	{
        public override void Load()
        {
            On_Main.RenderBackground += On_Main_RenderBackground;
        }
        private void On_Main_RenderBackground( On_Main.orig_RenderBackground orig, Main self )
        {
            if ( Main.npc != null && Main.npc.Any(npc => npc.active && npc.boss) )
            {
                self.GraphicsDevice.SetRenderTarget(null);

                self.GraphicsDevice.SetRenderTarget(self.backgroundTarget);
                self.GraphicsDevice.Clear(Color.Black);

                self.GraphicsDevice.SetRenderTarget(null);
            }
            else
            {
                orig.Invoke(self);
            }

        }

        public override void Unload()
        {
            On_Main.RenderBackground -= On_Main_RenderBackground;
        }
    }
}