using System.Collections.Generic;

namespace TeleportMenu
{
    public static class RCC_SceneManagerExtensions
    {
        public static RCC_CarControllerV3[] get_AllPlayerVehicles(this RCC_SceneManager rcc)
        {
            List<RCC_CarControllerV3> result = new List<RCC_CarControllerV3>(1);
            RCC_CarControllerV3[] allcars = UnityEngine.Object.FindObjectsOfType<RCC_CarControllerV3>();
            foreach (RCC_CarControllerV3 car in allcars)
            {
                if (car.name.Contains("(Clone)"))
                {
                    result.Add(car);
                }
            }
            return result.ToArray();
        }
        public static string[] get_AllPlayerNames(this RCC_SceneManager rcc)
        {
            RCC_CarControllerV3[] allvehicles = rcc.get_AllPlayerVehicles();
            List<string> result = new List<string>(allvehicles.Length - 1);
            for (int i = 0; i < allvehicles.Length; i++)
            {
                if (allvehicles[i] != rcc.activePlayerVehicle)
                {
                    result.Add(allvehicles[i].get_PlayerName());
                }
            }
            return result.ToArray();
        }
        public static RCC_CarControllerV3 get_PlayerVehicleByPlayerName(this RCC_SceneManager rcc, string name)
        {
            foreach (RCC_CarControllerV3 car in rcc.get_AllPlayerVehicles())
            {
                if (car.get_PlayerName() == name)
                {
                    return car;
                }
            }
            return null;
        }
    }
}
