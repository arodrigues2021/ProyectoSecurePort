using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
   
    public class  GruposTypeConfig : EntityTypeConfiguration<Grupos>
    {
        public GruposTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Grupos", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
            
            this.Property(t => t.Nombre)
               .IsRequired()
               .HasColumnName("Nombre");
            
            this.Property(t => t.id_usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
               .IsRequired()
               .HasColumnName("fech_alta");

            this.Property(t => t.es_activo)
               .IsRequired()
               .HasColumnName("es_activo");

            this.Property(t => t.fech_activo)
               .HasColumnName("fech_activo");

            this.Property(t => t.descripcion)
               .HasColumnName("descripcion");

            this.Property(t => t.Id_publico)
             .HasColumnName("Id_publico");

        }
    
    }

}
