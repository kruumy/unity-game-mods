using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace DashOverhaul
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class DashOverhaul : Mod
	{
        public static ModKeybind DashKey;
        public override void Load()
        {
            DashKey = KeybindLoader.RegisterKeybind(this, "Dash", Keys.LeftShift);
        }
        public override void Unload()
        {
            DashKey = null;
        }
    }
}
