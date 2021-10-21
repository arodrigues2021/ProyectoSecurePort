using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  CiudadesTypeConfig : EntityTypeConfiguration<Ciudades>
    {
        public CiudadesTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMAE_Ciudades", "dbo")
                 .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("id");

            // Properties
            this.Property(t => t.codigo)
                .IsRequired()
                .HasColumnName("codigo");

            this.Property(t => t.nombre)
                .IsRequired()
                .HasColumnName("nombre");

            this.Property(t => t.Id_provincia)
               .IsRequired()
               .HasColumnName("Id_provincia");
            
            this.Property(t => t.id_usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
               .HasColumnName("fech_alta");

            this.Property(t => t.es_activo)
                .IsRequired()
               .HasColumnName("es_activo");

            this.Property(t => t.fech_activo)
                .IsRequired()
               .HasColumnName("fech_activo");

            this.Property(t => t.Id_isla)
               .HasColumnName("id_isla");
            
        }
    
    }

}
