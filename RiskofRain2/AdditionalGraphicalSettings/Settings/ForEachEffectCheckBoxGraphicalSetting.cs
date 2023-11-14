using System;
using UnityEngine.Rendering.PostProcessing;

namespace AdditionalGraphicalSettings.Settings
{
    public abstract class ForEachEffectCheckBoxGraphicalSetting<T> : CheckBoxGraphicalSetting where T : PostProcessEffectSettings
    {
        protected ForEachEffectCheckBoxGraphicalSetting( bool defaultValue, string settingName, string settingDescription ) : base(defaultValue, settingName, settingDescription)
        {
        }
        protected override void OnCheckBoxChanged( bool newValue )
        {
            ForEachEffect(( T setting ) =>
            {
                setting.active = newValue;
                setting.enabled.value = newValue;
                setting.enabled.overrideState = true;
            });
        }
        protected void ForEachEffect( Action<T> action )
        {
            foreach ( UnityEngine.Object settingObj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(T)) )
            {
                T setting = (T)settingObj;
                action.Invoke(setting);
            }
        }
    }
}
