namespace StatEditor
{
    public static class CarControllerExtensions
    {
        public static float get_orgSteerAngle(this RCC_CarControllerV3 vehicle)
        {
            return (float)typeof(RCC_CarControllerV3).GetField("orgSteerAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(vehicle);
        }

        public static void set_orgSteerAngle(this RCC_CarControllerV3 vehicle, float value)
        {
            typeof(RCC_CarControllerV3).GetField("orgSteerAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(vehicle, value);
        }
    }
}
