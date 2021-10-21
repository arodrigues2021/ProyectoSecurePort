using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  CentrosTypeConfig : EntityTypeConfiguration<Centros>
    {
        public CentrosTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Centros_24H", "dbo")
                 .HasKey(t => t.id)
                 .Property(t => t.id)
                 .HasColumnName("id");

            // Properties
            this.Property(t => t.Id_Organismo)
                .IsRequired()
                .HasColumnName("Id_Organismo");

            this.Property(t => t.Id_Puerto)
                .HasColumnName("Id_Puerto");

            this.Property(t => t.Centro_24H)
                .IsRequired()
                .HasColumnName("Centro_24H");
            
            this.Property(t => t.id_usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");

            this.Property(t => t.Id_Ciudad)
               .HasColumnName("Id_Ciudad");

            this.Property(t => t.Id_Provincia)
               .HasColumnName("Id_Provincia");

            this.Property(t => t.Direccion)
               .HasColumnName("Direccion");

            this.Property(t => t.Cod_Postal)
               .HasColumnName("Cod_Postal");
            
        }
    
    }

}
