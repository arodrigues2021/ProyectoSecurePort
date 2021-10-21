using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class ComunicacionesTypeConfig : EntityTypeConfiguration<Comunicaciones>
    {
        public ComunicacionesTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Comunicaciones", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
            this.Property(t => t.Id_Puerto)
                .IsRequired()
                .HasColumnName("Id_Puerto");

            this.Property(t => t.Id_IIPP)
                .HasColumnName("Id_IIPP");
        
            this.Property(t => t.Asunto)
                .IsRequired()
                .HasColumnName("Asunto");

            this.Property(t => t.Fecha)
                .IsRequired()
                .HasColumnName("Fecha");

            this.Property(t => t.Emisor)
                .IsRequired()
                .HasColumnName("Emisor");
            
            this.Property(t => t.Receptor)
                .IsRequired()
                .HasColumnName("Receptor");

            this.Property(t => t.Mensaje)
                .HasColumnName("Mensaje");
            
            this.Property(t => t.Recibido)
                .HasColumnName("Recibido");

            this.Property(t => t.Id_Usu_Alta)
                .HasColumnName("Id_Usu_Alta");

            this.Property(t => t.Fech_alta)
                .IsRequired()
                .HasColumnName("Fech_alta");
     
        }
    }
}
