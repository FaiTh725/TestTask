namespace Application.Shared.Exceptions
{
    public class InternalServerApiException : Exception
    {
        public InternalServerApiException(string message) :
            base(message)
        {
        }
    }
}
