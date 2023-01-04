using EasyIMGUI.Controls.Automatic;
using UnityEngine;

namespace SaveEditor
{
    public static class GUI
    {
        public static readonly EasyIMGUI.MelonLoader.Interface.Menu Root = new EasyIMGUI.MelonLoader.Interface.Menu();
        public static void Initialize()
        {
            Root.Controls.Add(new Window()
            {
                Content =
                {
                    text = "Save Editor"
                },
                Dimensions = new Rect(10, 10, 225, 100),
                Controls =
                {
                    new Horizontal()
                    {
                        Controls =
                        {
                            new ButtonAndTextField
                            {
                                Value = PlayerSaveWrapper.MyBalance.ToString(),
                                OnButtonClick = x => PlayerSaveWrapper.MyBalance = x,
                                Content = { text = "Set MyBalance" }
                            }
                        }
                    },
                    new Horizontal()
                    {
                        Controls =
                        {
                            new ButtonAndTextField
                            {
                                Value = PlayerSaveWrapper.MyLvl.ToString(),
                                OnButtonClick = x => PlayerSaveWrapper.MyLvl = x,
                                Content = { text = "Set MyLvl" }
                            }
                        }
                    },
                    new Horizontal()
                    {
                        Controls =
                        {
                            new ButtonAndTextField
                            {
                                Value = PlayerSaveWrapper.BoostQuantity.ToString(),
                                OnButtonClick = x => PlayerSaveWrapper.BoostQuantity = x,
                                Content = { text = "Set BoostQuantity" }
                            }
                        }
                    }
                }
            });
        }
    }
}
