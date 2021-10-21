using System;
namespace SecurePort.Entities.Exceptions
{

    public class InvalidOperationException : ApplicationException
    {
        public InvalidOperationException(Exception exception)
            : base(BuildExceptionMessageWithErrors(exception)) { }

        private static string BuildExceptionMessageWithErrors(Exception exception)
        {
            return string.Format("{0}", exception.Message);
        }

    }

}

