using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class Tipos_DocumentoTypeConfig : EntityTypeConfiguration<Tipos_Documento>
    {

        public Tipos_DocumentoTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Tipos_Documento", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("Id");

            this.Property(t => t.documento)
               .IsRequired()
               .HasColumnName("Documento");

            this.Property(t => t.descripcion)
                .IsRequired()
               .HasColumnName("Descripcion");

            this.Property(t => t.acceso)
              .HasColumnName("Acceso");
            
        }

    }

}
