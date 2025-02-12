namespace Application.Shared.Exceptions
{
    public class AplicationConfigurationException : Exception
    {
        public string ErrorConfigurationSection { get; } = string.Empty;

        public AplicationConfigurationException(
            string errorconfigurationSection,
            string message = "") : 
            base(message)
        {
            ErrorConfigurationSection = errorconfigurationSection;
        }
    }
}
