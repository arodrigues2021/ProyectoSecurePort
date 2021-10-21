using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class IslasTypeConfig : EntityTypeConfiguration<Islas>
    {
        public IslasTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMAE_Islas", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("Id");

            //Properties
            this.Property(t => t.nombre)
                .IsRequired()
                .HasColumnName("nombre");
        
        }
    }
}
