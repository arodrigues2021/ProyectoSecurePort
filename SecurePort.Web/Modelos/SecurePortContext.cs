using System.Data.Entity;

namespace WebSecurePort.Models
{
  
    public class SecurePortContext : DbContext
    {
        public SecurePortContext()
            : base("SecurePortContext")
            {
            }

            public static SecurePortContext Create()
            {
                return new SecurePortContext();
            }

            public DbSet<Models.Puertos> Puertos { get; set; }

            public DbSet<Models.Usuarios> Usuarios { get; set; }

    }
}


