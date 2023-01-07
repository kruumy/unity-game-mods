using TMPro;

namespace TeleportMenu
{
    public static class RCC_CarControllerV3Extensions
    {
        /// Does not work on main player car.
        public static string get_PlayerName(this RCC_CarControllerV3 car)
        {
            try
            {
                UnityEngine.Transform TextTMP = car.transform.Find("Text (TMP)");
                TextMeshPro textObj = TextTMP.gameObject.GetComponent<TextMeshPro>();
                int startOfName = textObj.text.LastIndexOf('>') + 2;
                return textObj.text.Substring(startOfName);
            }
            catch
            {
                return null;
            }
        }

        public static bool IsPlayerCar(this RCC_CarControllerV3 car)
        {
            return car.name.Contains("(Clone)");
        }
    }
}
