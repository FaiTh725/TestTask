namespace Application.Shared.Exceptions
{
    public class BadRequestApiException : Exception
    {
        public BadRequestApiException(string message) :
            base(message)
        {
            
        }
    }
}
