using System.Linq;
using UnityEngine.Rendering.PostProcessing;

namespace AdditionalGraphicalSettings.Settings
{
    public class ColorGrading : CheckBoxGraphicalSetting
    {
        public ColorGrading() : base(true, "Color Grading", string.Empty)
        {
        }

        protected override void OnCheckBoxChanged( bool newValue )
        {
            PostProcessVolume GlobalPostProcessVolume = UnityEngine.Object.FindObjectsOfType<PostProcessVolume>().FirstOrDefault(p => p.name == "GlobalPostProcessVolume");
            UnityEngine.Rendering.PostProcessing.ColorGrading colorGrade = GlobalPostProcessVolume.profile.GetSetting<UnityEngine.Rendering.PostProcessing.ColorGrading>();
            colorGrade.active = newValue;
            colorGrade.enabled.value = newValue;
            colorGrade.enabled.overrideState = true;
        }
    }
}
