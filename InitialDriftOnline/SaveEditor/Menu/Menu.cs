using MelonLoader;
using UnityEngine;

namespace SaveEditor.Menu
{
    internal static class Menu
    {
        internal static Rect Dimensions = new Rect(10, 10, 300, 100);
        private static readonly int WindowID = new System.Random().Next();
        internal static bool IsOpen { get; private set; } = false;

        internal static void Open()
        {
            MelonEvents.OnGUI.Subscribe(Draw);
            IsOpen = true;

            MyBalanceText = PlayerSaveWrapper.MyBalance.ToString();
            MyLvlText = PlayerSaveWrapper.MyLvl.ToString();
            BoostQuantityText = PlayerSaveWrapper.BoostQuantity.ToString();
        }

        internal static void Close()
        {
            MelonEvents.OnGUI.Unsubscribe(Draw);
            IsOpen = false;
        }


        private static string MyBalanceText;
        private static string MyLvlText;
        private static string BoostQuantityText;
        private static void Draw()
        {
            Dimensions = GUI.Window(WindowID, Dimensions, (int windowId) =>
            {
                Contents.Generics.ButtonAndTextField(ref MyBalanceText, x => PlayerSaveWrapper.MyBalance = x, "Set MyBalance");
                Contents.Generics.ButtonAndTextField(ref MyLvlText, x => PlayerSaveWrapper.MyLvl = x, "Set MyLvl");
                Contents.Generics.ButtonAndTextField(ref BoostQuantityText, x => PlayerSaveWrapper.BoostQuantity = x, "Set BoostQuantity");
                GUI.DragWindow(new Rect(0, 0, Dimensions.width, 20));
            }, "Save Editor");
        }
    }
}