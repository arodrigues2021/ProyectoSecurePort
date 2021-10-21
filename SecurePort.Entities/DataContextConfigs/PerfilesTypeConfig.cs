using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class PerfilesTypeConfig : EntityTypeConfiguration<Perfiles>
    {

        public PerfilesTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Perfiles", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
            
            this.Property(t => t.Nombre)
               .IsRequired()
               .HasColumnName("Nombre");

            this.Property(t => t.id_usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
               .HasColumnName("fech_alta");

            this.Property(t => t.es_activo)
               .HasColumnName("es_activo");

            this.Property(t => t.fech_activo)
               .HasColumnName("fech_activo");
            
            this.Property(t => t.descripcion)
               .HasColumnName("Descripcion");


        }
    
    }

}
