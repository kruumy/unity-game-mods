using CodeStage.AntiCheat.Storage;

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
                ObscuredPrefs.SetInt("XP", value * 100);
            }
        }
        public static int XP
        {
            get => ObscuredPrefs.GetInt("XP");
            set
            {
                ObscuredPrefs.SetInt("MyLvl", value / 100);
                ObscuredPrefs.SetInt("XP", value);
            }
        }
        public static int MyBalance
        {
            get => ObscuredPrefs.GetInt("MyBalance");
            set => ObscuredPrefs.SetInt("MyBalance", value);
        }
        public static int BoostQuantity
        {
            get => ObscuredPrefs.GetInt("BoostQuantity");
            set => ObscuredPrefs.SetInt("BoostQuantity", value);
        }
    }
}
