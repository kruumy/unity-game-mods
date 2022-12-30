using MelonLoader;
using UnityEngine;

namespace Unstucker
{
    public class Main : MelonMod
    {
        public SRFreeCam FreeCam => Object.FindObjectOfType<SRFreeCam>();
        public SRAdminTools AdminTools => Object.FindObjectOfType<SRAdminTools>();
        public RCC_CarControllerV3 PlayerCar => RCC_SceneManager.Instance.activePlayerVehicle;
        public Vector3 PlayerCarPostion
        {
            get => new Vector3(
                RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position.x,
                RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position.y,
                RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position.z);
            set => RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position = value;
        }
        private readonly float Increment = 10f;
        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector3 cords = PlayerCarPostion;
                cords.y += Increment;
                PlayerCarPostion = cords;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector3 cords = PlayerCarPostion;
                cords.y -= Increment;
                PlayerCarPostion = cords;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector3 cords = PlayerCarPostion;
                cords.x += Increment;
                PlayerCarPostion = cords;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector3 cords = PlayerCarPostion;
                cords.x -= Increment;
                PlayerCarPostion = cords;
            }
            else if (Input.GetKeyDown(KeyCode.RightShift))
            {
                Vector3 cords = PlayerCarPostion;
                cords.z += Increment;
                PlayerCarPostion = cords;
            }
            else if (Input.GetKeyDown(KeyCode.RightControl))
            {
                Vector3 cords = PlayerCarPostion;
                cords.z -= Increment;
                PlayerCarPostion = cords;
            }

            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AdminTools.GoIroDown();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AdminTools.GoAkinaDown();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AdminTools.GoAkagiUp();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AdminTools.GoUSUI();
            }
            else if (Input.GetKeyDown(KeyCode.RightAlt))
            {
                FreeCam.FreeCamEnabled = !FreeCam.FreeCamEnabled;
            }
        }
    }
}
