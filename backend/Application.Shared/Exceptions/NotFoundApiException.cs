﻿namespace Application.Shared.Exceptions
{
    public class NotFoundApiException : Exception
    {
        public NotFoundApiException(string message) :
            base(message)
        {
            
        }
    }
}
