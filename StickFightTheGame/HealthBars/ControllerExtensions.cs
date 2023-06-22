namespace HealthBars
{
    public static class ControllerExtensions
    {
        public static T GetPrivateField<T>( this Controller controller, string fieldName )
        {
            System.Reflection.FieldInfo fightingField = typeof(Controller).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)fightingField.GetValue(controller);
        }

        public static HealthHandler GetHealthHandler( this Controller controller )
        {
            return controller.gameObject.GetComponent<HealthHandler>();
        }
    }

}
