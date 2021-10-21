using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  CapitaniaTypeConfig : EntityTypeConfiguration<Capitanias>
    {
        public CapitaniaTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMAE_Capitanias_Maritimas", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

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

            
        }
    
    }

}
