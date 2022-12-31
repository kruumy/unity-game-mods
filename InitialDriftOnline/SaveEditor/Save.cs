using CodeStage.AntiCheat.Storage;
using MelonLoader;

namespace SaveEditor
{
    public static class Save
    {
        public static int MyLvl
        {
            get => ObscuredPrefs.GetInt("MyLvl");
            set
            {
                ObscuredPrefs.SetInt("MyLvl", value);
                MelonLogger.Msg($"MyLvl = {value}");
                ObscuredPrefs.SetInt("XP", value * 100);
                MelonLogger.Msg($"XP = {value * 100}");
            }
        }

        public static int XP
        {
            get => ObscuredPrefs.GetInt("XP");
            set
            {
                ObscuredPrefs.SetInt("MyLvl", value / 100);
                MelonLogger.Msg($"MyLvl = {value / 100}");
                ObscuredPrefs.SetInt("XP", value);
                MelonLogger.Msg($"XP = {value}");
            }
        }

        public static int MyBalance
        {
            get => ObscuredPrefs.GetInt("MyBalance");
            set
            {
                ObscuredPrefs.SetInt("MyBalance", value);
                MelonLogger.Msg($"MyBalance = {value}");
            }
        }

        public static int BoostQuantity
        {
            get => ObscuredPrefs.GetInt("BoostQuantity");
            set
            {
                ObscuredPrefs.SetInt("BoostQuantity", value);
                MelonLogger.Msg($"BoostQuantity = {value}");
            }
        }
    }
}