using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(HideDoubleCash.Main), "HideDoubleCash", "1.0.0", "kruumy")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace HideDoubleCash
{
    public class Main : MelonMod
    {
        public override void OnSceneWasLoaded( int buildIndex, string sceneName )
        {
            GameObject TextUIElement = GameObject.Find("InGame/UIRect/MainHudLeftAlign(Clone)/LeftGroup/LayoutGroup/CashGroup/DoubleCashMode/Txt");
            if( TextUIElement != null )
            {
                TextUIElement.active = false;
            }
        }
    }
}
