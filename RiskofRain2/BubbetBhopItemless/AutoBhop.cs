using BepInEx.Configuration;
using EntityStates;

namespace BubbetBhopItemless
{
    public static class AutoBhop
    {
        public static ConfigEntry<bool> EnableAutoBhop;
        public static void Init( ConfigFile config )
        {
            EnableAutoBhop = config.Bind<bool>("General", "Enable AutoBhop", true, "Hold down [JUMP] to auto jump instead of timing jumps for bhops.");
            if ( EnableAutoBhop.Value == true )
            {
                On.EntityStates.GenericCharacterMain.GatherInputs += GenericCharacterMain_GatherInputs;
            }
        }

        private static void GenericCharacterMain_GatherInputs( On.EntityStates.GenericCharacterMain.orig_GatherInputs orig, GenericCharacterMain self )
        {
            orig.Invoke(self);
            if ( self.hasInputBank && self.isGrounded)
            {
                self.jumpInputReceived = self.inputBank.jump.down;
            }
        }
    }
}
