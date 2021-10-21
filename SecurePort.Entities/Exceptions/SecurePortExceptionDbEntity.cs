
namespace SecurePort.Entities.Exceptions
{
    #region using
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Text;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    #endregion
    
    public class SecureportExceptionDbEntity : DbEntityValidationException
    {
        public SecureportExceptionDbEntity(DbEntityValidationException exception)
            : base(BuildExceptionMessageWithErrors(exception)) { }

        private static string BuildExceptionMessageWithErrors(DbEntityValidationException exception)
        {
            return string.Format("{0} EntityValidationErrors: {1}", exception.Message, GetErrorsSerialized(exception));
        }

        private static string GetErrorsSerialized(DbEntityValidationException exception)
        {
            var errors = EnumerateErrorMessages(exception);

            var result = Serialize(errors);

            return result;
        }

        private static string Serialize(IEnumerable<string> errors)
        {
            var builder = new StringBuilder();
            foreach (var error in errors)
            {
                builder.AppendFormat("{0};", error);
            }
            return builder.ToString();
        }

        private static IEnumerable<string> EnumerateErrorMessages(DbEntityValidationException exception)
        {
            foreach (var result in exception.EntityValidationErrors)
            {
                foreach (var error in result.ValidationErrors)
                {
                    yield return string.Format("{0} : {1}", error.PropertyName, error.ErrorMessage);
                }
            }
        }
    }


    public class SecureportExceptionModelValidation : ModelValidationException
    {
        public SecureportExceptionModelValidation(ModelValidationException exception)
            : base(BuildExceptionMessageWithErrors(exception)) { }

        private static string BuildExceptionMessageWithErrors(ModelValidationException exception)
        {
            return string.Format("{0} EntityValidationErrors: {1}", exception.Message, GetErrorsSerialized(exception));
        }

        private static string GetErrorsSerialized(ModelValidationException exception)
        {
            var errors = EnumerateErrorMessages(exception);

            var result = Serialize(errors);

            return result;
        }

        private static string Serialize(IEnumerable<string> errors)
        {
            var builder = new StringBuilder();
            foreach (var error in errors)
            {
                builder.AppendFormat("{0};", error);
            }
            return builder.ToString();
        }
        
        private static IEnumerable<string> EnumerateErrorMessages(ModelValidationException ex)
        {
            var results = new List<string>() { ex.Message };
            var innerEx = ex.InnerException;
            while (innerEx != null) 
            {
                results.Add(innerEx.Message);
                innerEx = innerEx.InnerException;
            }
            return results;
        }
    }
  

    public class SecureportExceptionDbUpdate : DbUpdateException
    {
        public SecureportExceptionDbUpdate(DbUpdateException exception)
            : base(BuildExceptionMessageWithErrors(exception)) { }

        private static string BuildExceptionMessageWithErrors(DbUpdateException exception)
        {
            return string.Format("{0} EntityValidationErrors: {1}", exception.Message, GetErrorsSerialized(exception));
        }

        private static string GetErrorsSerialized(DbUpdateException exception)
        {
            var errors = EnumerateErrorMessages(exception);

            var result = Serialize(errors);

            return result;
        }

        private static string Serialize(IEnumerable<string> errors)
        {
            var builder = new StringBuilder();
            foreach (var error in errors)
            {
                builder.AppendFormat("{0};", error);
            }
            return builder.ToString();
        }
        
        private static IEnumerable<string> EnumerateErrorMessages(DbUpdateException ex)
        {
            var results = new List<string>() { ex.Message };
            var innerEx = ex.InnerException;
            while (innerEx != null) 
            {
                results.Add(innerEx.Message);
                innerEx = innerEx.InnerException;
            }
            return results;
        }
    }

}

