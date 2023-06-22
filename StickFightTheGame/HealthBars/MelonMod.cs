using MelonLoader;

namespace HealthBars
{
    public class Melon : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HealthBars.Show();
        }
    }
}