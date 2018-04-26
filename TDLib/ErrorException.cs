using System;
using TD;

namespace TD
{
    public class ErrorException : Exception
    {
        public readonly Error Error;

        public ErrorException(Error error) : base(error.Message)
        {
            Error = error;
        }
    }
}