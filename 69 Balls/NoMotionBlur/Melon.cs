using MelonLoader;
using System.Timers;

namespace NoMotionBlur
{
    public class Melon : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Timer Timer = new Timer(1000);
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
            MelonLogger.Msg("Started NoMotionBlur Timer");
        }

        private void Timer_Elapsed( object sender, ElapsedEventArgs e )
        {
            try
            {
                foreach ( UnityEngine.Rendering.Universal.MotionBlur item in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Rendering.Universal.MotionBlur)) )
                {
                    if ( item.active )
                    {
                        item.active = false;
                        MelonLogger.Msg("Set MotionBlur Object To False");
                    }
                }
            }
            catch
            {

            }

        }
    }
}
