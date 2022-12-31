namespace StatEditor
{
    public static class PlayerVehicleWrapper
    {
        private static RCC_CarControllerV3 Vehicle => RCC_SceneManager.Instance.activePlayerVehicle;
        public static float DownForce
        {
            get => (float)(Vehicle?.downForce);
            set => Vehicle.downForce = value;
        }
        public static float Torque
        {
            get => Vehicle.engineTorque;
            set => Vehicle.engineTorque = value;
        }
        public static float Brake
        {
            get => Vehicle.brakeTorque;
            set => Vehicle.brakeTorque = value;
        }
        public static float MaxSpeed
        {
            get => Vehicle.maxspeed;
            set => Vehicle.maxspeed = value;
        }
        public static float SteerAngle
        {
            get => (float)typeof(RCC_CarControllerV3).GetField("orgSteerAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(Vehicle);
            set => typeof(RCC_CarControllerV3).GetField("orgSteerAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(Vehicle, value);
        }
    }
}
