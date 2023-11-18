using RoR2.UI;
using System;
using System.Linq;
using UnityEngine;

namespace AdditionalGraphicalSettings.MenuAPI
{
    public abstract class MenuControl<T, SettingT> : MenuBase, IResetToDefault where SettingT : BaseSettingsControl
    {
        public T value;
        public T defaultValue;
        protected string token;
        public T revertValue;
        public SettingT controller;

        protected MenuControl( T defaultValue, string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings = true ) : base(settingName, settingDescription, panelLocation, showInPauseSettings)
        {
            this.defaultValue = defaultValue;
            token = settingName.Replace(" ", string.Empty);
            if ( !PlayerPrefs.HasKey(token) )
            {
                PlayerPrefsSet(defaultValue);
            }
            SetValue(PlayerPrefsGet());
            On.RoR2.UI.SettingsPanelController.RevertChanges += SettingsPanelController_RevertChanges;
            RoR2.CameraRigController.onCameraEnableGlobal += CameraRigController_onCameraEnableGlobal;
        }

        private void CameraRigController_onCameraEnableGlobal( RoR2.CameraRigController obj )
        {
            // temp fix to invoke onvaluechanged for each control on game start
            SetValue(GetValue());
        }
        private void SettingsPanelController_RevertChanges( On.RoR2.UI.SettingsPanelController.orig_RevertChanges orig, SettingsPanelController self )
        {
            if ( !controller ) return;
            if ( self.settingsControllers != null && self.settingsControllers.Contains(controller) )
            {
                SetValue(revertValue);
            }
            orig(self);
        }

        protected MenuControl( T defaultValue, string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings, ValueChanged callback ) : this(defaultValue, settingName, settingDescription, panelLocation, showInPauseSettings)
        {
            OnValueChanged += callback;
        }

        public T GetValue()
        {
            return value;
        }

        public virtual void SetValue( T newValue )
        {
            value = newValue;
            PlayerPrefsSet(value);
            InvokeOnValueChanged(value);
        }

        private void PlayerPrefsSet( T value )
        {
            switch ( value )
            {
                case bool b:
                    PlayerPrefs.SetInt(token, Convert.ToInt32(b));
                    break;
                case string s:
                    PlayerPrefs.SetString(token, s);
                    break;
                case int i:
                    PlayerPrefs.SetInt(token, i);
                    break;
                case float f:
                    PlayerPrefs.SetFloat(token, f);
                    break;
                case double d:
                    PlayerPrefs.SetFloat(token, (float)d);
                    break;
                default:
                    throw new ArgumentException("Unsupported type");
            }
        }
        private T PlayerPrefsGet()
        {
            if ( typeof(T) == typeof(bool) || typeof(T) == typeof(int) )
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetInt(token), typeof(T));
            }
            else if ( typeof(T) == typeof(string) )
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetString(token), typeof(T));
            }
            else if ( typeof(T) == typeof(float) || typeof(T) == typeof(double) )
            {
                return (T)Convert.ChangeType(PlayerPrefs.GetFloat(token), typeof(T));
            }
            else
            {
                throw new ArgumentException("Unsupported type");
            }
        }


        public delegate void ValueChanged( T newValue );
        public event ValueChanged OnValueChanged;
        protected void InvokeOnValueChanged( T newValue )
        {
            OnValueChanged?.Invoke(newValue);
        }

        public void ResetToDefault()
        {
            SetValue(defaultValue);
        }
    }
}
