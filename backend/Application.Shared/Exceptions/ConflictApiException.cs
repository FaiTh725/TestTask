namespace Application.Shared.Exceptions
{
    public class ConflictApiException : Exception
    {
        public ConflictApiException(string message) : 
            base(message)
        {
            
        }
    }
}
