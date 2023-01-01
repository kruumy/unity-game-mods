using MelonLoader;

namespace StatEditor
{
    public static class Preferences
    {
        public static MelonPreferences_Category MainCatagory { get; } = MelonPreferences.CreateCategory(nameof(StatEditor));
        public static MelonPreferences_Entry<float> downForce { get; } = MainCatagory.CreateEntry(nameof(downForce), -1f);
        public static MelonPreferences_Entry<float> engineTorque { get; } = MainCatagory.CreateEntry(nameof(engineTorque), -1f);
        public static MelonPreferences_Entry<float> brakeTorque { get; } = MainCatagory.CreateEntry(nameof(brakeTorque), -1f);
        public static MelonPreferences_Entry<float> maxspeed { get; } = MainCatagory.CreateEntry(nameof(maxspeed), -1f);
        public static MelonPreferences_Entry<float> orgSteerAngle { get; } = MainCatagory.CreateEntry(nameof(orgSteerAngle), -1f);

        public static void Save()
        {
            downForce.Value = RCC_SceneManager.Instance.activePlayerVehicle.downForce;
            engineTorque.Value = RCC_SceneManager.Instance.activePlayerVehicle.engineTorque;
            brakeTorque.Value = RCC_SceneManager.Instance.activePlayerVehicle.brakeTorque;
            maxspeed.Value = RCC_SceneManager.Instance.activePlayerVehicle.maxspeed;
            orgSteerAngle.Value = RCC_SceneManager.Instance.activePlayerVehicle.get_orgSteerAngle();
            MelonPreferences.Save();
        }
        public static void Load()
        {
            MelonPreferences.Load();
            if (downForce.DefaultValue != downForce.Value)
                RCC_SceneManager.Instance.activePlayerVehicle.downForce = downForce.Value;
            if (engineTorque.DefaultValue != engineTorque.Value)
                RCC_SceneManager.Instance.activePlayerVehicle.engineTorque = engineTorque.Value;
            if (brakeTorque.DefaultValue != brakeTorque.Value)
                RCC_SceneManager.Instance.activePlayerVehicle.brakeTorque = brakeTorque.Value;
            if (maxspeed.DefaultValue != maxspeed.Value)
                RCC_SceneManager.Instance.activePlayerVehicle.maxspeed = maxspeed.Value;
            if (orgSteerAngle.DefaultValue != orgSteerAngle.Value)
                RCC_SceneManager.Instance.activePlayerVehicle.set_orgSteerAngle(orgSteerAngle.Value);
        }
    }
}
