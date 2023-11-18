using AdditionalGraphicalSettings.MenuAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdditionalGraphicalSettings.Settings
{
    public class Sun : Setting
    {
#nullable enable
        private Light? OldSun { get; set; }
        private Light? NewSun { get; set; }
        private NGSS_Directional? NewSunNGSS => NewSun != null ? NewSun.gameObject.GetComponent<NGSS_Directional>() : null;
#nullable disable

        public MenuCheckbox Override { get; }
        public MenuSlider Intensity { get; }
        public MenuSlider Red { get; }
        public MenuSlider Green { get; }
        public MenuSlider Blue { get; }
        public MenuSlider ShadowSoftness { get; }
        public Sun()
        {
            SceneManager.activeSceneChanged += ( Scene _, Scene _ ) => { CopySun(); };

            Override = new MenuCheckbox(false, "Override Sun", "Override the current stage's sun. Some shadow settings above will NOT apply until the next stage is loaded or the recopy sun button is pressed. This is because the sun object gets copied at the start of the stage.", SubPanel.Graphics, true, ( bool newValue ) =>
            {
                if ( OldSun != null && NewSun != null )
                {
                    OldSun.gameObject.SetActive(!newValue);
                    NewSun.gameObject.SetActive(newValue);
                }
            });
            Intensity = new MenuSlider(1, 10, 0, false, "Sun Intensity", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                if ( NewSun != null )
                {
                    NewSun.intensity = newValue;
                }
            });
            Red = new MenuSlider(1, 1, 0, false, "Sun Color Red", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                if ( NewSun != null )
                {
                    NewSun.color = new Color(newValue, NewSun.color.g, NewSun.color.b, NewSun.color.a);
                }
            });
            Green = new MenuSlider(1, 1, 0, false, "Sun Color Green", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                if ( NewSun != null )
                {
                    NewSun.color = new Color(NewSun.color.r, newValue, NewSun.color.b, NewSun.color.a);
                }
            });
            Blue = new MenuSlider(1, 1, 0, false, "Sun Color Blue", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                if ( NewSun != null )
                {
                    NewSun.color = new Color(NewSun.color.r, NewSun.color.g, newValue, NewSun.color.a);
                }
            });
            ShadowSoftness = new MenuSlider(1, 35, 0, false, "Sun Shadow Softness", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                if ( NewSunNGSS != null )
                {
                    NewSunNGSS.NGSS_SHADOWS_SOFTNESS = newValue;
                }
            });

        }

        private void CopySun()
        {
            if ( NewSun != null && NewSunNGSS != null )
            {
                Object.Destroy(NewSunNGSS);
                Object.Destroy(NewSun);
                Log.LogMessage("Destroyed previous duplicated sun.");
            }

            if ( OldSun != null )
            {
                OldSun.gameObject.SetActive(true);
            }
            OldSun = GameObject.Find("Directional Light (SUN)")?.GetComponent<Light>();
            if ( OldSun == null )
            {
                Log.LogMessage("Could not find sun object");
                return;
            }
            NewSun = Object.Instantiate(OldSun);
            OldSun.gameObject.SetActive(!Override.GetValue());
            NewSun.gameObject.SetActive(Override.GetValue());
        }
    }
}
