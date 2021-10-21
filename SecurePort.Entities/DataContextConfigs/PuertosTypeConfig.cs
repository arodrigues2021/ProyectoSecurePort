using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class PuertosTypeConfig : EntityTypeConfiguration<Puertos>
    {
        public PuertosTypeConfig()
        {

            // Primary Key
            this.ToTable("SPMAE_Puertos", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasColumnName("Nombre");
            
            // Properties
            this.Property(t => t.id_Organismo)
                .IsRequired()
                .HasColumnName("id_Organismo");
            
            // Properties
            this.Property(t => t.Responsable)
                .IsRequired()
                .HasColumnName("Responsable");

            // Properties
            this.Property(t => t.Direccion)
                .HasColumnName("Direccion");

            // Properties
            this.Property(t => t.ID_Ciudad)
                .HasColumnName("ID_Ciudad");

            // Properties
            this.Property(t => t.Cod_Postal)
                .HasColumnName("Cod_Postal");

            // Properties
            this.Property(t => t.ID_Provincia)
                .HasColumnName("ID_Provincia");

            // Properties
            this.Property(t => t.ID_Usu_alta)
                .HasColumnName("ID_Usu_alta");

            // Properties
            this.Property(t => t.Fech_Alta)
                .HasColumnName("Fech_Alta");

            // Properties
            this.Property(t => t.es_activo)
                .HasColumnName("es_activo");

            // Properties
            this.Property(t => t.fech_activo)
                .HasColumnName("fech_activo");

            // Properties
            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");

            // Properties
            this.Property(t => t.Latitud)
                .HasColumnName("Latitud");

            // Properties
            this.Property(t => t.Longitud)
                .HasColumnName("Longitud");

            // Properties
            this.Property(t => t.Locode)
                .HasColumnName("Locode");

            // Properties
            this.Property(t => t.Id_CapMarit)
                .HasColumnName("Id_CapMarit");
            
        }
    }
}