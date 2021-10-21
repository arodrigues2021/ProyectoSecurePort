using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class Tipos_ServicioTypeConfig : EntityTypeConfiguration<Tipos_Servicio>
    {

        public Tipos_ServicioTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Tipos_Servicio", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("Id");

            this.Property(t => t.servicio)
               .IsRequired()
               .HasColumnName("Documento");

            this.Property(t => t.descripcion)
                .IsRequired()
               .HasColumnName("Descripcion");

        }

    }

}
