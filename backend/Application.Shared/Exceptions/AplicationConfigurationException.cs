namespace Application.Shared.Exceptions
{
    public class AplicationConfigurationException : Exception
    {
        public required string ErrorConfigurationSection { get; set; }

        public AplicationConfigurationException(
            string errorconfigurationSection,
            string message = "") : 
            base(message)
        {
            ErrorConfigurationSection = errorconfigurationSection;
        }
    }
}
