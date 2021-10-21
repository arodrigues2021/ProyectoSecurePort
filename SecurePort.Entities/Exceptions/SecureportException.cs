using System;
namespace SecurePort.Entities.Exceptions
{
 
    public class SecureportException : ApplicationException
    {
        public SecureportException(Exception exception)
            : base(BuildExceptionMessageWithErrors(exception)) { }

       
        private static string BuildExceptionMessageWithErrors(Exception exception)
        {
            return string.Format("{0}", exception.Message);
        }
        
    }

}

