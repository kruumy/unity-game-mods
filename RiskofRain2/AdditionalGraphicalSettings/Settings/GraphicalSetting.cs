namespace AdditionalGraphicalSettings.Settings
{
    public abstract class GraphicalSetting
    {
        protected GraphicalSetting()
        {
            On.RoR2.CameraRigController.OnEnable += ( On.RoR2.CameraRigController.orig_OnEnable orig, RoR2.CameraRigController self ) =>
            {
                orig.Invoke(self);
                OnCameraSpawn();
            };
        }
        protected virtual void OnCameraSpawn()
        {
        }
    }
}
