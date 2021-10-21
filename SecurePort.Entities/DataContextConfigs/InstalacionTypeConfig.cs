using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class InstalacionTypeConfig : EntityTypeConfiguration<Tipos_Instalacion>
    {
        public InstalacionTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMAE_Tipos_Instalacion", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnName("Descripcion");

            this.Property(t => t.Clasificacion)
                .HasColumnName("Clasificacion");

           // Properties
           this.Property(t => t.id_usu_alta)
                .HasColumnName("id_usu_alta");

           // Properties
           this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");
            
           // Properties
           this.Property(t => t.es_activo)
                .HasColumnName("es_activo");

           // Properties
           this.Property(t => t.fech_activo)
                .HasColumnName("fech_activo");
           
        }
    }
}
