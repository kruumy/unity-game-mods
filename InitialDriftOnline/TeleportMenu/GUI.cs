using EasyIMGUI.Controls.Automatic;
using System;
using UnityEngine;

namespace TeleportMenu
{
    public static class GUI
    {
        public static readonly EasyIMGUI.MelonLoader.Interface.Menu Root = new EasyIMGUI.MelonLoader.Interface.Menu();
        public static void Initialize()
        {
            SingleButton btn1 = new SingleButton();
            btn1.OnButtonPressed += (object ob, EventArgs e) => Teleport.UpTofu();
            btn1.Content.text = "Up Tofu";
            SingleButton btn2 = new SingleButton();
            btn2.Content.text = "Down Tofu";
            btn2.OnButtonPressed += (object ob, EventArgs e) => Teleport.DownTofu();

            TextField PlayerNameField = new TextField();
            PlayerNameField.Value = string.Empty;

            SingleButton btn3 = new SingleButton();
            btn3.Content.text = "To Player";
            btn3.OnButtonPressed += (object ob, EventArgs e) => Teleport.ToPlayer(PlayerNameField.Value);

            Root.Controls.Add(new Window()
            {
                Content =
                {
                    text = "Teleport Menu"
                },
                Dimensions = new Rect(10, 10, 300, 10),
                Controls =
                {
                    btn1,
                    btn2,
                    new Horizontal()
                    {
                        Controls =
                        {
                            btn3,
                            PlayerNameField
                        }
                    }
                }
            });
        }
    }
}
