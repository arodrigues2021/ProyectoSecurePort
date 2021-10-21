using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;


namespace SecurePort.Entities.DataContextConfigs
{
    public class Tipos_InstalacionTypeConfig : EntityTypeConfiguration<Tipos_Instalacion>
    {
        public Tipos_InstalacionTypeConfig()
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

        }
    }
}
