namespace HealthBars
{
    public static class ControllerExtensions
    {
        public static HealthHandler GetHealthHandler( this Controller controller )
        {
            return controller.gameObject.GetComponent<HealthHandler>();
        }
    }

}
