using MelonLoader;

namespace HealthBars
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HealthBars.Show();
        }
    }
}