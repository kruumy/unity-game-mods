using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using MonoMod.RuntimeDetour;

namespace DashImmunity
{
    public class DashImmunity : Mod
    {
        // Does not work with Calamity Dashes
        /*
        public override void Load()
        {
            Terraria.On_Player.DoCommonDashHandle += On_Player_DoCommonDashHandle;
        }

        public override void Unload()
        {
            Terraria.On_Player.DoCommonDashHandle -= On_Player_DoCommonDashHandle;
        }
        private void On_Player_DoCommonDashHandle(On_Player.orig_DoCommonDashHandle orig, Player self, out int dir, out bool dashing, Player.DashStartAction dashStartAction)
        {
            orig(self, out dir, out dashing, dashStartAction);
            if (dashing)
            {
                self.immune = true;
                self.immuneTime = 20;
            }
        }
        */
    }
}