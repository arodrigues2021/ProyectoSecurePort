
using System;
using System.Data.Entity;

public class ValidateDatabaseInitializer<TContext> : IDatabaseInitializer<TContext>
        where TContext : DbContext
{
    public void InitializeDatabase(TContext context)
    {
        try
        {
            if (!context.Database.Exists())
            {
                throw new ApplicationException(string.Format("Instancia de DB '{0}' no existe.", context.Database.Connection.Database));
            }
        }
        catch (Exception)
        {

            return ;
        }
        
    }
}
