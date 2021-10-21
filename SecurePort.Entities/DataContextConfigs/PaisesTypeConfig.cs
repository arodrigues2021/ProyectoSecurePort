using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;


namespace SecurePort.Entities.DataContextConfigs
{
    public class PaisesTypeConfig : EntityTypeConfiguration<Paises>
    {
        public PaisesTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMAE_Paises", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
             this.Property(t => t.nombre)
                .IsRequired()
                .HasColumnName("nombre");
            
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

            this.Property(t => t.cod_pais)
               .IsRequired()
               .HasColumnName("cod_pais");
            
        }
    }
}
