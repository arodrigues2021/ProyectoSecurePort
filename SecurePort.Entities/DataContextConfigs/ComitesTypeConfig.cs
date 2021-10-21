using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  ComitesTypeConfig : EntityTypeConfiguration<Comites>
    {
        public ComitesTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Comites", "dbo")
                 .HasKey(t => t.id)
                 .Property(t => t.id)
                 .HasColumnName("id");

            // Properties
            this.Property(t => t.Id_Organismo)
                .IsRequired()
                .HasColumnName("Id_Organismo");

            this.Property(t => t.Id_Puerto)
                .IsRequired()
                .HasColumnName("Id_Puerto");

            this.Property(t => t.Nivel)
                .IsRequired()
                .HasColumnName("Nivel");

            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");
            
            this.Property(t => t.id_usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");

            this.Property(t => t.Convocado)
                .IsRequired()
                .HasColumnName("Convocado");
            
        }
    
    }

}
