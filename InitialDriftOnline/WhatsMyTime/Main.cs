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
            SRToffuManagerWrapper.OnTofuRunStopped += OnTofuRunStopped; ;
        }

        private void GuiWork()
        {
            string elapsedTime = string.Format("{0:000}.{1:000}",
            SRToffuManagerWrapper.Timer.Elapsed.TotalSeconds,
            SRToffuManagerWrapper.Timer.Elapsed.Milliseconds);
            GUI.Label(LabelRect, elapsedTime, LabelStyle);
        }

        private void OnTofuRunStart(object sender, System.EventArgs e)
        {
            MelonEvents.OnGUI.Subscribe(GuiWork);
        }

        private async void OnTofuRunStopped(object sender, System.EventArgs e)
        {
            await Task.Delay(10000);
            MelonEvents.OnGUI.Unsubscribe(GuiWork);
        }
    }
}
