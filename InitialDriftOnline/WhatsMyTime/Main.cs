using MelonLoader;
using System.Threading.Tasks;
using UnityEngine;

namespace WhatsMyTime
{
    public class Main : MelonMod
    {
        private readonly Rect LabelRect = new Rect((Screen.width / 2) - 65, Screen.height - 35, 0, 0);

        private readonly GUIStyle LabelStyle = new GUIStyle()
        {
            fontSize = 35,
            fontStyle = FontStyle.BoldAndItalic,
            normal = new GUIStyleState() { textColor = Color.white },
            wordWrap = false
        };

        public override void OnInitializeMelon()
        {
            SRToffuManagerWrapper.Initialize();
            SRToffuManagerWrapper.OnTofuRunStart += OnTofuRunStart;
            SRToffuManagerWrapper.OnTofuRunCompleted += OnTofuRunCompleted;
            SRToffuManagerWrapper.OnTofuRunInterupted += OnTofuRunInterupted;
        }

        private void GuiWork()
        {
            string elapsedTime = string.Format("{0:000}.{1:000}",
            SRToffuManagerWrapper.Timer.Elapsed.TotalSeconds,
            SRToffuManagerWrapper.Timer.Elapsed.Milliseconds);
            GUI.Label(LabelRect, elapsedTime, LabelStyle);
        }

        private async void OnTofuRunCompleted(object sender, System.EventArgs e)
        {
            MelonLogger.Msg("Tofu Run Completed :)");
            MelonLogger.Msg($"Time Elapsed = {SRToffuManagerWrapper.Timer.Elapsed}");
            await Task.Delay(10000);
            MelonEvents.OnGUI.Unsubscribe(GuiWork);
        }

        private void OnTofuRunInterupted(object sender, System.EventArgs e)
        {
            MelonEvents.OnGUI.Unsubscribe(GuiWork);
        }

        private void OnTofuRunStart(object sender, System.EventArgs e)
        {
            MelonEvents.OnGUI.Subscribe(GuiWork);
        }
    }
}
