using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class CNAETypeConfig : EntityTypeConfiguration<CNAE>
    {
        public CNAETypeConfig()
        {
            // Primary Key
            this.ToTable("CNAE","dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
            this.Property(t => t.IdCodigo)
                .IsRequired()
                .HasColumnName("IdCodigo");

            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnName("Descripcion");
            
        }
    }
}


